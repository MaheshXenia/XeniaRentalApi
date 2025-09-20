using Stripe;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.TenantAssignment
{
    public interface ITenantAssignmentRepository
    {
        Task<IEnumerable<Models.TenantAssignemnt>> GetTenantAssignments();
        Task<PagedResultDto<Models.TenantAssignemnt>> GetTenantAssignmentsByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.TenantAssignemnt> CreateTenantAssignments(DTOs.TenantAssignment assigment);

        Task<bool> DeleteTenantAssignment(int id);

        Task<IEnumerable<DTOs.TenantAssignmentDTO>> GetTenantAssignemntsbyId(int documassignmentId);

        Task<bool> UpDateTenantAssignment(TenantAssignmentUpdateDTO assignemnt);

        Task<IEnumerable<Models.TenantAssignemnt>> GetAllCloseAgreemnts();
        Task<IEnumerable<Models.TenantAssignemnt>> GetAllCloseAgreemntsByParams(DateTime startDate, DateTime endDate, int propId, int unitId);

        Task<Models.TenantAssignemnt> CreateCloseAgreements(DTOs.TenantAssignment assigment);

        Task<PagedResultDto<Models.TenantAssignemnt>> GetTenantAssignmentAsync(string? search, int pageNumber, int pageSize);

        Task<Models.TenantAssignemnt> AddTenantAssignmentWithDocumentsAsync(TenantAssignmentDocumentUploadDTO dto);

        Task<PagedResultDto<Models.TenantAssignemnt>> GetCloseAgreementsByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<bool> UpDateCloseAssignment(int id, Models.TenantAssignemnt assignemnt);

        Task<TenantAssignmentDocumentUploadDTO> GetTenantAssignemntDocumentsbyId(int documassignmentId,int docType);
    }
}
