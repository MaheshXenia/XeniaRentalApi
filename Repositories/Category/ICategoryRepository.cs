using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<XRS_Categories>> GetCategories();

        Task<PagedResultDto<XRS_Categories>> GetCategorybyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<IEnumerable<XRS_Categories>> GetCategorybyId(int Id);

        Task<XRS_Categories> CreateCategory(CategoryDto category);

        Task<bool> UpdateCategory(int id, CategoryDto bedSpace);

        Task<bool> DeleteCategory(int id);

    }
}
