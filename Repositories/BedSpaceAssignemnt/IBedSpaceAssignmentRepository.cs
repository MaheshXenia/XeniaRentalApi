using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.BedSpaceAssignemnt
{
    public interface IBedSpaceAssignmentRepository
    {
        Task<IEnumerable<Models.BedSpaceAssignemnt>> GetBedSpaceAssigments();
        Task<PagedResultDto<Models.BedSpaceAssignemnt>> GetBedSpaceAssignemntsByCompanyId(int companyId, string srch,int pageNumber, int pageSize);

        Task<Models.BedSpaceAssignemnt> CreateBedSpaceAssignemnt(DTOs.BedSpaceAssignment bedSpaceAssignemnt);

        Task<bool> DeleteBedSpaceAssignment(int id);
        Task<IEnumerable<Models.BedSpaceAssignemnt>> GetBedSpaceAssignmentbyId(int bedSpaceAssignmentId);//UpdateBedSpaceAssignment

        Task<bool> UpdateBedSpaceAssignment(int id, Models.BedSpaceAssignemnt bedSpace);

        Task<PagedResultDto<Models.BedSpaceAssignemnt>> GetBedSpaceAssignmentAsync(string? propertyName, string unitName, string tenantName, string contactNo, int pageNumber, int pageSize);
    }
}
