using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.MessDetails
{
    public interface IMessDetailsRepository
    {
        Task<IEnumerable<Models.MessDetails>> GetMessDetails();
        Task<PagedResultDto<Models.MessDetails>> GetMessDetailsByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.MessDetails> CreateMessDetails(XeniaRentalApi.Models.MessDetails messTypes);
        Task<IEnumerable<Models.MessDetails>> GetMessDetailsbyId(int messdetailId);

        Task<bool> UpdateMessDetails(int id, Models.MessDetails details);


    }
}
