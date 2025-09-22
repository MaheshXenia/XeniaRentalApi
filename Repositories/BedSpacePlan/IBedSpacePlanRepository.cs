using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.BedSpacePlan
{
    public interface IBedSpacePlanRepository
    {
        Task<IEnumerable<object>> GetAllAsync();          
        Task<object?> GetByIdAsync(int bedPlanID);        
        Task<IEnumerable<object>> GetByCompanyIdAsync(int companyID);
        Task<XRS_BedSpacePlan> CreateAsync(XRS_BedSpacePlan entity);
        Task<bool> UpdateAsync(XRS_BedSpacePlan entity);
        Task<bool> DeleteAsync(int bedID);

    }
}