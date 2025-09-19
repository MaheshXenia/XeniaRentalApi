using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.TenantDocuments
{
    public interface ITenantDocumentRepository
    {
        Task<IEnumerable<Models.TenantDocuments>> GetTenantDocuments();
        Task<PagedResultDto<Models.TenantDocuments>> GetTenantDocumentsByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.TenantDocuments> CreateTenantDocuments(DTOs.CreateTenantDocuments document);

        Task<bool> DeleteTenantDocument(int id);

        Task<IEnumerable<Models.TenantDocuments>> GetTenantDocumentsbyId(int documentId);

        Task<PagedResultDto<Models.TenantDocuments>> GetTenantDocumentsAsync(string? search, int pageNumber, int pageSize);

        Task<Dictionary<string, string>> UploadFilesAsync(List<IFormFile> files);

        Task<bool> UpDateTenantDocument(int id, Models.TenantDocuments documents);

    }
}
