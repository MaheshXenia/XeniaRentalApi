using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.MessTypes
{
    public interface IMessTypes
    {
        Task<IEnumerable<Models.MessTypes>> GetMessTypes();
        Task<PagedResultDto<Models.MessTypes>> GetMessTypesByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.MessTypes> CreateMessTypes(DTOs.CreateMessTypes messTypes);

        Task<bool> DeleteMessType(int id);
        Task<IEnumerable<Models.MessTypes>> GetMessTypesbyId(int messTypeId);

        Task<bool> UpdatMessTypes(int id, Models.MessTypes types);
    }
}
