using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Properties
{
    public interface IPropertiesRepository
    {
        Task<IEnumerable<Models.Properties>> GetProperties();
        Task<PagedResultDto<Models.Properties>> GetPropertiesByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Properties> CreateProperties(DTOs.CreateProperties property);

        Task<bool> DeleteProperty(int id);

        Task<IEnumerable<Models.Properties>> GetPrpoertiesbyId(int propertyId);

        Task<bool> UpDateProperties(int id, Models.Properties properties);

        Task<PagedResultDto<Models.Properties>> GetPropertiesAsync(string? search, int pageNumber, int pageSize);


    }
}
