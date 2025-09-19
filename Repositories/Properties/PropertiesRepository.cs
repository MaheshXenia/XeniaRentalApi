using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;


namespace XeniaRentalApi.Repositories.Properties
{
    public class PropertiesRepository: IPropertiesRepository
    {
        private readonly ApplicationDbContext _context;
        public PropertiesRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.Properties>> GetProperties()
        {

            return await _context.Properties.ToListAsync();

        }


        public async Task<PagedResultDto<Models.Properties>> GetPropertiesByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.CompanyId.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
               .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
                .Select(u => new Models.Properties
                {
                    PropID = u.PropID,
                    propertyName = u.propertyName,
                    propertyType = u.propertyType,
                    IsActive = u.IsActive,
                    CompanyId = u.CompanyId

                }).ToListAsync();
            return new PagedResultDto<Models.Properties>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };



        }

        public async Task<IEnumerable<Models.Properties>> GetPrpoertiesbyId(int propertyId)
        {

            return await _context.Properties
                .Where(u => u.PropID == propertyId)
                 .ToListAsync();

        }

        public async Task<Models.Properties> CreateProperties(DTOs.CreateProperties dtoProperties)
        {

            var properties = new Models.Properties
            {
                propertyName = dtoProperties.propertyName,
                propertyType = dtoProperties.propertyType,
                CompanyId =   dtoProperties.CompanyId,
                IsActive = dtoProperties.IsActive

            };

            await _context.Properties.AddAsync(properties);
            await _context.SaveChangesAsync();
            return properties;

        }

        public async Task<bool> DeleteProperty(int id)
        {
            var bedspacesettings = await _context.Properties.FirstOrDefaultAsync(u => u.PropID == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpDateProperties(int id, Models.Properties properties)
        {
            var updateProperties = await _context.Properties.FirstOrDefaultAsync(u => u.PropID == id);
            if (updateProperties == null) return false;

            updateProperties.propertyName = properties.propertyName;
            updateProperties.propertyType = properties.propertyType;
            updateProperties.CompanyId = properties.CompanyId;
            updateProperties.IsActive = properties.IsActive;
          
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.Properties>> GetPropertiesAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.propertyName.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.Properties
                {
                    PropID = u.PropID,
                    propertyName= u.propertyName,
                    propertyType=u.propertyType,
                    CompanyId = u.CompanyId,
                    IsActive = u.IsActive,


                })
                .ToListAsync();

            return new PagedResultDto<Models.Properties>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
