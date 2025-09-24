using Stripe;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.TenantAssignment
{
    public interface ITenantAssignmentRepository
    {

        Task<IEnumerable<TenantAssignmentGetDto>> GetByCompanyAllId(int companyId);
        Task<IEnumerable<TenantAssignmentGetDto>> GetByCompanyIdAsync(int companyId, bool isBedSpace = false);
        Task<TenantAssignmentGetDto?> GetByIdAsync(int tenantAssignId);
        Task<XRS_TenantAssignment> CreateAsync(TenantAssignmentCreateDto dto);
        Task<bool> UpdateAsync(int tenantAssignId, TenantAssignmentCreateDto dto);
        Task<bool> UpdateClosureAsync(int tenantAssignId, TenantClosureCreateDto dto);
        Task<bool> DeleteAsync(int tenantAssignId);
    }
}
