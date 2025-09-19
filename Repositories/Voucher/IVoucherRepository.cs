using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Voucher
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Models.Voucher>> GetVouchers();
        Task<IEnumerable<Models.Voucher>> GetVoucherByCompanyId(int companyId);
        
        Task<Models.Voucher> CreateVoucher(XeniaRentalApi.DTOs.CreateVoucher voucher);

        Task<IEnumerable<Models.Voucher>> GetVoucherIdById(int voucherId);

        Task<bool> UpdateVoucher(int id, Models.Voucher voucher);
        Task<IEnumerable<Models.Voucher>> GetPaymentStatus();

        Task<PagedResultDto<Models.Voucher>> GetVochersAsync(string? search, int pageNumber, int pageSize);


    }
}
