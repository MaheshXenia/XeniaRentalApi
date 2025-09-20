using Stripe;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.TenantAssignment
{
    public interface ITenantAssignmentRepository
    {
        Task<IEnumerable<TenantAssignmentGetDto>> GetByCompanyIdAsync(int companyId);
        Task<TenantAssignmentGetDto?> GetByIdAsync(int tenantAssignId);
        Task<XRS_TenantAssignment> CreateAsync(TenantAssignmentCreateDto dto);
        Task<XRS_TenantAssignment?> UpdateAsync(TenantAssignmentCreateDto dto);
        Task<bool> DeleteAsync(int tenantAssignId);
    }
}
