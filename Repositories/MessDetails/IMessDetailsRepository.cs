using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.MessDetails
{
    public interface IMessDetailsRepository
    {
        Task<IEnumerable<Models.XRS_MessDetails>> GetMessDetails();
        Task<PagedResultDto<Models.XRS_MessDetails>> GetMessDetailsByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.XRS_MessDetails> CreateMessDetails(XeniaRentalApi.Models.XRS_MessDetails messTypes);
        Task<IEnumerable<Models.XRS_MessDetails>> GetMessDetailsbyId(int messdetailId);

        Task<bool> UpdateMessDetails(int id, Models.XRS_MessDetails details);


    }
}
