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
        Task<XRS_Voucher?> GetVoucherByIdAsync(int id);
        Task<IEnumerable<XRS_Voucher>> GetAllVouchersAsync(int companyId);
        Task<XRS_Voucher?> UpdateVoucherAsync(int id, VoucherDto dto);
        Task<bool> DeleteVoucherAsync(int id);


    }
}
