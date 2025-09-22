using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Voucher
{
    public class VoucherRrepository:IVoucherRepository
    {
        private readonly ApplicationDbContext _context;
        public VoucherRrepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.XRS_Voucher>> GetVouchers()
        {

            return await _context.Vouchers.ToListAsync();

        }

        public async Task<IEnumerable<Models.XRS_Voucher>> GetPaymentStatus()
        {

            return await _context.Vouchers.ToListAsync();

        }


        public async Task<IEnumerable<Models.XRS_Voucher>> GetVoucherByCompanyId(int companyId)
        {

            return await _context.Vouchers
                .Where(u => u.CompanyID == companyId)
                 .ToListAsync();

        }

        public async Task<IEnumerable<Models.XRS_Voucher>> GetVoucherIdById(int voucherId)
        {

            return await _context.Vouchers
                .Where(u => u.VoucherID == voucherId)
                 .ToListAsync();

        }

        public async Task<Models.XRS_Voucher> CreateVoucher(DTOs.CreateVoucher dtoVoucher)
        {

            var voucher = new Models.XRS_Voucher
            {
                unitID = dtoVoucher.unitID,
                CompanyID = dtoVoucher.CompanyID,
                PropID = dtoVoucher.PropID,
                VoucherNo = dtoVoucher.VoucherNo,
                VoucherDate = dtoVoucher.VoucherDate,
                VoucherType = dtoVoucher.VoucherType,
                DrID=dtoVoucher.DrID,
                CrID=dtoVoucher.CrID,
                Amount=dtoVoucher.Amount,
                RefNo=dtoVoucher.RefNo,
                Remarks=dtoVoucher.Remarks,
                IssueingBank=dtoVoucher.IssueingBank,
                ChequeNo=dtoVoucher.ChequeNo,
                Cancelled=dtoVoucher.Cancelled,
                CrAmount=dtoVoucher.CrAmount,
                IsReconcil=dtoVoucher.IsReconcil,
                ChequeStatus=dtoVoucher.ChequeStatus,
                ReconcilDate=dtoVoucher.ReconcilDate,
                paidByUser=dtoVoucher.paidByUser,
                isActive=dtoVoucher.isActive,



            };
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
            return voucher;

        }


       
        public async Task<bool> UpdateVoucher(int id, Models.XRS_Voucher voucher)
        {
            var updateVoucher = await _context.Vouchers.FirstOrDefaultAsync(u => u.VoucherID == id);
            if (updateVoucher == null) return false;

            updateVoucher.VoucherDate = voucher.VoucherDate;
            updateVoucher.VoucherNo = voucher.VoucherNo;
            updateVoucher.ChequeNo = voucher.ChequeNo;
            updateVoucher.Cancelled = voucher.Cancelled;
            updateVoucher.Amount= voucher.Amount;
            updateVoucher.RefNo = voucher.RefNo;
            updateVoucher.ReconcilDate = voucher.ReconcilDate;
            updateVoucher.ChequeStatus = voucher.ChequeStatus;
            updateVoucher.CompanyID = voucher.CompanyID;
            updateVoucher.CrID = voucher.CrID;
            updateVoucher.DrID = voucher.DrID;
            updateVoucher.CrAmount = voucher.CrAmount;
            updateVoucher.IsReconcil = voucher.IsReconcil;
            updateVoucher.IssueingBank = voucher.IssueingBank;
            updateVoucher.PropID = voucher.PropID;
            updateVoucher.Remarks = voucher.Remarks;
            updateVoucher.paidByUser = voucher.paidByUser;
            updateVoucher.isActive = voucher.isActive;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.XRS_Voucher>> GetVochersAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Vouchers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.VoucherNo.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.XRS_Voucher
                {
                    PropID = u.PropID,
                    VoucherNo = u.VoucherNo,
                    VoucherType = u.VoucherType,
                    VoucherDate = u.VoucherDate,
                    Amount = u.Amount,  
                    isActive = u.isActive,

                })
                .ToListAsync();

            return new PagedResultDto<Models.XRS_Voucher>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
