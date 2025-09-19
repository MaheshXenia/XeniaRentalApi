using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.Category>> GetCategories()
        {

            return await _context.Category
                .Where(u => u.IsActive == true)
                 .Select(u => new Models.Category
                 {
                     CategoryName = u.CategoryName,
                     CatID = u.CatID,
                     CompanyID = u.CompanyID,
                     IsActive = u.IsActive,

                 }).ToListAsync();


        }

        public async Task<PagedResultDto<Models.Category>> GetCategorybyCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.Category.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.CompanyID.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.CategoryName) // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                 .Select(u => new Models.Category
                 {
                     CategoryName = u.CategoryName,
                     CatID = u.CatID,
                     CompanyID = u.CompanyID,
                     IsActive = u.IsActive,

                 }).ToListAsync();


            return new PagedResultDto<Models.Category>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

        }

        public async Task<IEnumerable<Models.Category>> GetCategorybyId(int categoryId)
        {

            return await _context.Category
                .Where(u => u.CatID == categoryId)
                 .Select(u => new Models.Category
                 {
                     CategoryName = u.CategoryName,
                     CatID = u.CatID,
                     CompanyID = u.CompanyID,
                     IsActive = u.IsActive,

                 }).ToListAsync();

        }

        public async Task<Models.Category> CreateCategory(DTOs.CreateCategory dtoCategory)
        {

            var category = new Models.Category
            {
                CategoryName = dtoCategory.CategoryName,
                IsActive = dtoCategory.IsActive,
                CompanyID = dtoCategory.CompanyID,

            };
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;

        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Category.FirstOrDefaultAsync(u => u.CatID == id);
            if (category == null) return false;
            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategory(int id, Models.Category category)
        {
            var updatebedSpace = await _context.Category.FirstOrDefaultAsync(u => u.CatID == id);
            if (updatebedSpace == null) return false;

            updatebedSpace.CategoryName = category.CategoryName;
            updatebedSpace.CompanyID = category.CompanyID;
            updatebedSpace.IsActive = category.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.Category>> GetCategoriesAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Category.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.CategoryName.Contains(search)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.CategoryName) // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.Category
                {
                    CategoryName = u.CategoryName,
                    CatID = u.CatID,
                    CompanyID = u.CompanyID,
                    IsActive = u.IsActive,

                }).ToListAsync();

            return new PagedResultDto<Models.Category>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
