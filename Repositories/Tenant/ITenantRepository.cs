using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Tenant
{
    public interface ITenantRepository
    {
        Task<IEnumerable<Models.Tenant>> GetTenants();
        Task<PagedResultDto<Models.Tenant>> GetTenantsByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Tenant> CreateTenant(XeniaRentalApi.DTOs.CreateTenant tenant);

        Task<TenantDocumentGetDTO> GetTenantsbyId(int tenantId);

        Task<bool> DeleteTenant(int id);

        Task<bool> UpDateTenant(int id, Models.Tenant tenant);

        Task<PagedResultDto<Models.Tenant>> GetTenantAsync(string? search, int pageNumber, int pageSize);

        Task<Dictionary<string, string>> UploadFilesAsync(List<IFormFile> files);

        Task<Models.Tenant> AddTenantWithDocumentsAsync(TenantWithDocumentsDto dto);

        Task<bool> UpDateTenantDocuments(DTOs.TenantDocumentDTO documentDTO);



    }
}
