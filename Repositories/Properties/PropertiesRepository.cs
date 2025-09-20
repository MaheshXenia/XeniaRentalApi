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

        public async Task<IEnumerable<XRS_Properties>> GetProperties(int companyId)
        {
            return await _context.Properties
                .Where(p => p.CompanyId == companyId) 
                .ToListAsync();
        }

        public async Task<PagedResultDto<XRS_Properties>> GetPropertiesByCompanyId(int companyId,string? search = null, int pageNumber = 1,int pageSize = 10)
        {
            var query = _context.Properties.AsQueryable();

            query = query.Where(u => u.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(u =>
                    u.propertyName.ToLower().Contains(lowerSearch) ||
                    u.propertyType.ToLower().Contains(lowerSearch));
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.propertyName) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new XRS_Properties
                {
                    PropID = u.PropID,
                    propertyName = u.propertyName,
                    propertyType = u.propertyType,
                    IsActive = u.IsActive,
                    CompanyId = u.CompanyId
                })
                .ToListAsync();

            return new PagedResultDto<XRS_Properties>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<XRS_Properties>> GetPrpoertiesbyId(int propertyId)
        {

            return await _context.Properties
                .Where(u => u.PropID == propertyId)
                 .ToListAsync();

        }

        public async Task<XRS_Properties> CreateProperties(XRS_Properties dtoProperties)
        {

            var properties = new XRS_Properties
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

        public async Task<bool> UpDateProperties(int id, XRS_Properties properties)
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

        public async Task<bool> DeleteProperty(int id)
        {
            var bedspacesettings = await _context.Properties.FirstOrDefaultAsync(u => u.PropID == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
