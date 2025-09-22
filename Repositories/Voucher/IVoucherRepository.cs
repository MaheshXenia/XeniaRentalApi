using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Voucher
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Models.XRS_Voucher>> GetVouchers();
        Task<IEnumerable<Models.XRS_Voucher>> GetVoucherByCompanyId(int companyId);
        
        Task<Models.XRS_Voucher> CreateVoucher(XeniaRentalApi.DTOs.CreateVoucher voucher);

        Task<IEnumerable<Models.XRS_Voucher>> GetVoucherIdById(int voucherId);

        Task<bool> UpdateVoucher(int id, Models.XRS_Voucher voucher);
        Task<IEnumerable<Models.XRS_Voucher>> GetPaymentStatus();

        Task<PagedResultDto<Models.XRS_Voucher>> GetVochersAsync(string? search, int pageNumber, int pageSize);


    }
}
