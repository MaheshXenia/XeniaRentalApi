using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<XRS_Voucher>> GetAllVouchersAsync(int companyId)
        {
            return await _context.Vouchers.ToListAsync();
        }


        public async Task<XRS_Voucher?> GetVoucherByIdAsync(int id)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.VoucherID == id);
        }


        public async Task<XRS_Voucher> CreateVoucherAsync(VoucherDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var indirectExpensesGroup = await _context.AccountGroups
                .FirstOrDefaultAsync(g => g.groupName == "Indirect Expenses" && g.companyID == dto.CompanyID);

                var drLedger = await _context.Ledgers
                 .FirstOrDefaultAsync(g => g.ledgerName == dto.DrID && g.companyID == dto.CompanyID);

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
                    CreatedOn = DateTime.Now,
                    CreatedBy = dto.CreatedBy ?? "System",
                    ModificationBy = dto.ModificationBy,
                    isActive = dto.IsActive
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();
           
            

                if (indirectExpensesGroup == null)
                    throw new Exception("Indirect Expenses account group not found.");
         
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
                    createdOn = DateTime.Now,
                    createdBy = dto.CreatedBy ?? "System",
                    modifiedOn = DateTime.Now,
                    modifiedBy = dto.CreatedBy ?? "System",
                    isActive = true
                };

    
                var creditEntry = new XRS_Accounts
                {
                    companyID = dto.CompanyID,
                    VoucherId = voucher.VoucherID,
                    GroupId = indirectExpensesGroup.groupID, 
                    invType = voucher.VoucherType,
                    invNo = voucher.VoucherNo,
                    invDate = voucher.VoucherDate,
                    ledgerDr = dto.CrID,
                    ledgerCr = drLedger.ledgerID,  
                    amountDr = voucher.Amount,
                    amountCr = 0,
                    remarks = "Indirect Expenses Voucher",
                    createdOn = DateTime.Now,
                    createdBy = dto.CreatedBy ?? "System",
                    isActive = true
                };

                _context.Accounts.Add(debitEntry);
                _context.Accounts.Add(creditEntry);
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
    

    }
}
