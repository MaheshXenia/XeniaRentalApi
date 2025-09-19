using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.BedSpacePlan
{
    public interface IBedSpacePlanRepository
    {
        Task<IEnumerable<Models.BedSpacePlan>> GetBedSpacePlans();
        Task<PagedResultDto<Models.BedSpacePlan>> GetBedSpacePlanbyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.BedSpacePlan> CreateBedSpacePlan(DTOs.CreateBedSpacePlan bedSpacePlan);

        Task<bool> DeleteBedSpacePlan(int id);

        Task<IEnumerable<Models.BedSpacePlan>> GetBedSpacePlanbyId(int bedSpacePlanId);

        Task<bool> UpdateBedSpacePlan(int id, Models.BedSpacePlan bedSpace);

        Task<PagedResultDto<Models.BedSpacePlan>> GetBedSpacePlanAsync(string? search, int pageNumber, int pageSize);
    }
}