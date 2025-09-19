using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Models.Category>> GetCategories();
        Task<PagedResultDto<Models.Category>> GetCategorybyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Category> CreateCategory(DTOs.CreateCategory category);

        Task<bool> DeleteCategory(int id);

        Task<IEnumerable<Models.Category>> GetCategorybyId(int Id);//UpdateBedSpace

        Task<bool> UpdateCategory(int id, Models.Category bedSpace);
        Task<PagedResultDto<Models.Category>> GetCategoriesAsync(string? search, int pageNumber, int pageSize);
    }
}
