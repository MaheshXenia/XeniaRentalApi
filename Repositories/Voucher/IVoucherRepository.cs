using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Voucher
{
    public interface IVoucherRepository
    {
        Task<XRS_Voucher> CreateVoucherAsync(VoucherDto dto);
        Task<object?> GetVoucherByIdAsync(int id);
        Task<IEnumerable<object>> GetAllVouchersAsync(int companyId);
        Task<XRS_Voucher?> UpdateVoucherAsync(int id, VoucherDto dto);
        Task<bool> DeleteVoucherAsync(int id);
        Task<XRS_Voucher> CreateIntiateAsync(VoucherCreateRequest request);
        Task<object> GetTenantChargesByMonthAsync(int month, int year);


    }
}
