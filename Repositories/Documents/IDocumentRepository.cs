using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Documents
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Models.Documents>> GetDocuments();
        Task<PagedResultDto<Models.Documents>> GetDocumentsCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Documents> CreateDocuments(DTOs.CreateDocuments documents);

        Task<bool> DeleteDocumentType(int id);

        Task<IEnumerable<Models.Documents>> GetDocumentsbyId(int documentId);

        Task<bool> UpdateDocuments(int id, Models.Documents documents);

        Task<PagedResultDto<Models.Documents>> GetDocumentsAsync(string? search, int pageNumber, int pageSize);
    }
}
