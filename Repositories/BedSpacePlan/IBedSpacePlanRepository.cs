using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.BedSpacePlan
{
    public interface IBedSpacePlanRepository
    {
        Task<IEnumerable<XRS_BedSpacePlan>> GetBedSpacePlans(int companyId);

        Task<PagedResultDto<XRS_BedSpacePlan>> GetBedSpacePlanByCompanyId(int companyId, string? search = null,int pageNumber = 1, int pageSize = 10);

        Task<XRS_BedSpacePlan> GetBedSpacePlanById(int bedSpacePlanId);

        Task<XRS_BedSpacePlan> CreateBedSpacePlan(XRS_BedSpacePlan bedSpacePlan);

        Task<bool> UpdateBedSpacePlan(int id, XRS_BedSpacePlan bedSpace);

        Task<bool> DeleteBedSpacePlan(int id);


    

    }
}