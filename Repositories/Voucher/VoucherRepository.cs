﻿using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Voucher
{
    public class VoucherRepository:IVoucherRepository
    {
        private readonly ApplicationDbContext _context;
        public VoucherRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<object>> GetAllVouchersAsync(int companyId)
        {
            var query = from v in _context.Vouchers
                        where v.CompanyID == companyId
                        join dr in _context.Ledgers on v.DrID equals dr.ledgerID
                        join cr in _context.Ledgers on v.CrID equals cr.ledgerID
                        select new
                        {
                            v.VoucherID,
                            v.VoucherNo,
                            v.VoucherDate,
                            v.VoucherType,
                            v.Amount,
                            v.RefNo,
                            v.Remarks,
                            v.IssueingBank,
                            v.ChequeNo,
                            v.Cancelled,
                            v.CrAmount,
                            v.IsReconcil,
                            v.ChequeStatus,
                            v.ReconcilDate,
                            v.CreatedOn,
                            v.CreatedBy,
                            v.ModificationBy,
                            v.isActive,
                            DrID = v.DrID,
                            DrName = dr.ledgerName,
                            CrID = v.CrID,
                            CrName = cr.ledgerName
                        };

            return await query.AsNoTracking().ToListAsync<object>();
        }



        public async Task<object?> GetVoucherByIdAsync(int id)
        {
            var query = from v in _context.Vouchers
                        where v.VoucherID == id
                        join dr in _context.Ledgers on v.DrID equals dr.ledgerID
                        join cr in _context.Ledgers on v.CrID equals cr.ledgerID
                        select new
                        {
                            v.VoucherID,
                            v.VoucherNo,
                            v.VoucherDate,
                            v.VoucherType,
                            v.Amount,
                            v.RefNo,
                            v.Remarks,
                            v.IssueingBank,
                            v.ChequeNo,
                            v.Cancelled,
                            v.CrAmount,
                            v.IsReconcil,
                            v.ChequeStatus,
                            v.ReconcilDate,
                            v.CreatedOn,
                            v.CreatedBy,
                            v.ModificationBy,
                            v.isActive,
                            DrID = dr.ledgerID,
                            DrName = dr.ledgerName,
                            CrID = cr.ledgerID,
                            CrName = cr.ledgerName
                        };

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<XRS_Voucher> CreateVoucherAsync(VoucherDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var indirectExpensesGroup = await _context.AccountGroups
                    .FirstOrDefaultAsync(g => g.groupName == "INDIRECT EXPENSES" && g.companyID == dto.CompanyID);

                if (indirectExpensesGroup == null)
                    throw new Exception("Indirect Expenses account group not found.");

                var drLedger = await _context.Ledgers
                    .FirstOrDefaultAsync(l => l.ledgerName == dto.DrID && l.companyID == dto.CompanyID);

                if (drLedger == null)
                    throw new Exception($"Ledger '{dto.DrID}' not found.");

                var now = DateTime.Now;

                var voucher = new XRS_Voucher
                {
                    unitID = dto.UnitID,
                    CompanyID = dto.CompanyID,
                    PropID = dto.PropID,
                    VoucherNo = dto.VoucherNo,
                    VoucherDate = dto.VoucherDate,
                    VoucherType = dto.VoucherType,
                    DrID = drLedger.ledgerID,
                    CrID = dto.CrID,
                    Amount = dto.Amount,
                    RefNo = dto.RefNo,
                    Remarks = dto.Remarks,
                    IssueingBank = dto.IssuingBank,
                    ChequeNo = dto.ChequeNo,
                    Cancelled = dto.Cancelled,
                    CrAmount = dto.CrAmount ?? 0,
                    IsReconcil = dto.IsReconcil,
                    ChequeStatus = dto.ChequeStatus,
                    ReconcilDate = dto.ReconcilDate,
                    CreatedOn = now,
                    CreatedBy = dto.CreatedBy ?? "System",
                    ModificationBy = dto.ModificationBy,
                    isActive = dto.IsActive
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();

                // Debit Entry
                var debitEntry = new XRS_Accounts
                {
                    companyID = dto.CompanyID,
                    VoucherId = voucher.VoucherID,
                    GroupId = indirectExpensesGroup.groupID,
                    invType = voucher.VoucherType,
                    invNo = voucher.VoucherNo,
                    invDate = voucher.VoucherDate,
                    ledgerDr = drLedger.ledgerID,
                    ledgerCr = dto.CrID,
                    amountDr = 0,
                    amountCr = voucher.Amount,
                    remarks = "Indirect Expenses Voucher",
                    createdOn = now,
                    createdBy = dto.CreatedBy ?? "System",
                    modifiedOn = now,
                    modifiedBy = dto.CreatedBy ?? "System",
                    isActive = true
                };

                // Credit Entry
                var creditEntry = new XRS_Accounts
                {
                    companyID = dto.CompanyID,
                    VoucherId = voucher.VoucherID,
                    GroupId = indirectExpensesGroup.groupID,
                    invType = voucher.VoucherType,
                    invNo = voucher.VoucherNo,
                    invDate =voucher.VoucherDate,
                    ledgerDr = dto.CrID,
                    ledgerCr = drLedger.ledgerID,
                    amountDr = voucher.Amount,
                    amountCr = 0,
                    remarks = "Indirect Expenses Voucher",
                    createdOn = now,
                    createdBy = dto.CreatedBy ?? "System",
                    modifiedOn = now,
                    modifiedBy = dto.CreatedBy ?? "System",
                    isActive = true
                };

                _context.Accounts.AddRange(debitEntry, creditEntry);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return voucher;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<XRS_Voucher?> UpdateVoucherAsync(int voucherId, VoucherDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.VoucherID == voucherId);
                if (voucher == null)
                    return null;


                var drLedger = await _context.Ledgers
                               .FirstOrDefaultAsync(g => g.ledgerName == dto.DrID && g.companyID == dto.CompanyID);

                voucher.VoucherNo = dto.VoucherNo ?? voucher.VoucherNo;
                voucher.VoucherDate = dto.VoucherDate;
                voucher.VoucherType = dto.VoucherType;
                voucher.DrID = drLedger.ledgerID;
                voucher.CrID = dto.CrID;
                voucher.Amount = dto.Amount;
                voucher.RefNo = dto.RefNo;
                voucher.Remarks = dto.Remarks;
                voucher.IssueingBank = dto.IssuingBank;
                voucher.ChequeNo = dto.ChequeNo;
                voucher.Cancelled = dto.Cancelled;
                voucher.CrAmount = dto.CrAmount ?? voucher.CrAmount;
                voucher.IsReconcil = dto.IsReconcil;
                voucher.ChequeStatus = dto.ChequeStatus;
                voucher.ReconcilDate = dto.ReconcilDate;
                voucher.ModificationBy = dto.ModificationBy ?? "System";
                voucher.ModifiedOn = DateTime.Now;

                var accounts = await _context.Accounts
                    .Where(a => a.VoucherId == voucher.VoucherID)
                    .ToListAsync();


                var debitEntry = accounts.FirstOrDefault(a => a.amountDr > 0);
                if (debitEntry != null)
                {
                    debitEntry.ledgerDr = drLedger.ledgerID;
                    debitEntry.ledgerCr = dto.CrID;
                    debitEntry.amountDr = dto.Amount;
                    debitEntry.amountCr = 0;
                    debitEntry.remarks = "Indirect Expenses Debit Entry (Updated)";
                    debitEntry.modifiedOn = DateTime.Now;
                    debitEntry.modifiedBy = dto.ModificationBy ?? "System";
                }

    
                var creditEntry = accounts.FirstOrDefault(a => a.amountCr > 0);
                if (creditEntry != null)
                {
                    creditEntry.ledgerDr = dto.CrID;
                    creditEntry.ledgerCr = drLedger.ledgerID;
                    creditEntry.amountDr = 0;
                    creditEntry.amountCr = dto.Amount;
                    creditEntry.remarks = "Cash/Bank Credit Entry (Updated)";
                    creditEntry.modifiedOn = DateTime.Now;
                    creditEntry.modifiedBy = dto.ModificationBy ?? "System";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return voucher;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.VoucherID == id);
            if (voucher == null) return false;

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetTenantChargesByMonthAsync(int month, int year)
        {
            var tenants = await _context.TenantAssignemnts
                .Include(t => t.Tenant)
                .Include(t => t.Unit)
                    .ThenInclude(u => u.Property)
                .Include(t => t.BedSpace)
                .Select(t => new
                {
                    t.tenantID,
                    TenantName = t.Tenant.tenantName,
                    t.unitID,
                    UnitName = t.Unit.UnitName,
                    PropertyName = t.Unit.Property.propertyName,
                    BedSpaceName = t.BedSpace != null ? t.BedSpace.bedSpaceName : null,
                    t.agreementStartDate,
                    t.rentCollection,
                    t.collectionType,
                    t.rentAmt
                })
                .ToListAsync();

            var result = new List<object>();

            foreach (var tenant in tenants)
            {
                DateTime nextDueDate = CalculateNextRentDueDate(
                    tenant.agreementStartDate,
                    tenant.rentCollection,
                    tenant.collectionType
                );

                var chargeIds = await _context.UnitChargesMappings
                    .Where(m => m.unitID == tenant.unitID)
                    .Select(m => m.chargeID)
                    .ToListAsync();

                var charges = await _context.Charges
                    .Where(c => chargeIds.Contains(c.chargeID))
                    .Select(c => new
                    {
                        c.chargeID,
                        c.chargeName,
                        c.chargeAmt,
                        c.isVariable
                    })
                    .ToListAsync();

                var variableCharges = charges.Where(c => c.isVariable).ToList();
                var fixedCharges = charges.Where(c => !c.isVariable).ToList();

                decimal totalCharges = tenant.rentAmt + variableCharges.Sum(c => c.chargeAmt) + fixedCharges.Sum(c => c.chargeAmt);

                result.Add(new
                {
                    tenant.tenantID,
                    tenant.TenantName,
                    tenant.unitID,
                    tenant.UnitName,
                    tenant.PropertyName,
                    tenant.BedSpaceName,
                    tenant.rentCollection,
                    Frequency = tenant.collectionType,
                    NextRentDueDate = nextDueDate,
                    VariableCharges = variableCharges,
                    FixedCharges = fixedCharges,
                    TotalCharges = totalCharges
                });
            }

            return result;
        }



        private DateTime CalculateNextRentDueDate(DateTime agreementStart, int dueDay, string frequency)
        {
            DateTime nextDate;

            switch (frequency.ToLower())
            {
                case "monthly":
                    nextDate = agreementStart.AddMonths(1);
                    break;

                case "quarterly":
                    nextDate = agreementStart.AddMonths(3);
                    break;

                case "yearly":
                    nextDate = agreementStart.AddYears(1);
                    break;

                default:
                    throw new ArgumentException("Invalid frequency type");
            }

          
            int daysInMonth = DateTime.DaysInMonth(nextDate.Year, nextDate.Month);
            int day = Math.Min(dueDay, daysInMonth);

            return new DateTime(nextDate.Year, nextDate.Month, day);
        }



    }
}
