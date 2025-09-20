using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Models.XRS_Categories>> GetCategories();
        Task<PagedResultDto<Models.XRS_Categories>> GetCategorybyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.XRS_Categories> CreateCategory(DTOs.CreateCategory category);

        Task<bool> DeleteCategory(int id);

        Task<IEnumerable<Models.XRS_Categories>> GetCategorybyId(int Id);//UpdateBedSpace

        Task<bool> UpdateCategory(int id, Models.XRS_Categories bedSpace);
        Task<PagedResultDto<Models.XRS_Categories>> GetCategoriesAsync(string? search, int pageNumber, int pageSize);
    }
}
