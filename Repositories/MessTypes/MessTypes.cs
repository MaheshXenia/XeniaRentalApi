using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.MessTypes
{
    public class MessTypes:IMessTypes
    {
        private readonly ApplicationDbContext _context;
        public MessTypes(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.MessTypes>> GetMessTypes()
        {

            return await _context.MessTypes.ToListAsync();

        }


        public async Task<PagedResultDto<Models.MessTypes>> GetMessTypesByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.MessTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.CompanyId == companyId); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
            .OrderBy(u => u.MessName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new Models.MessTypes
            {
              messID = u.messID,
              MessName=u.MessName,
              CompanyId=u.CompanyId,
              IsActive=u.IsActive,
               
            })
            .ToListAsync();

            return new PagedResultDto<Models.MessTypes>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<Models.MessTypes>> GetMessTypesbyId(int messTypeId)
        {

            return await _context.MessTypes
                .Where(u => u.messID == messTypeId)
                 .ToListAsync();

        }

        public async Task<Models.MessTypes> CreateMessTypes(DTOs.CreateMessTypes messTypes)
        {

            var messTypesInsert = new Models.MessTypes
            {
                MessName = messTypes.MessName,
                IsActive = messTypes.IsActive,
                CompanyId= messTypes.CompanyId,

            };

            await _context.MessTypes.AddAsync(messTypesInsert);
            await _context.SaveChangesAsync();
            return messTypesInsert;

        }

        public async Task<bool> DeleteMessType(int id)
        {
            var bedspacesettings = await _context.Documents.FirstOrDefaultAsync(u => u.companyID == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.isActive = false;
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatMessTypes(int id, Models.MessTypes types)
        {
            var updatedMessTypes = await _context.MessTypes.FirstOrDefaultAsync(u => u.messID == id);
            if (updatedMessTypes == null) return false;

            updatedMessTypes.MessName = types.MessName;
            updatedMessTypes.messID = types.messID;
            updatedMessTypes.CompanyId = types.CompanyId;
            updatedMessTypes.IsActive = types.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
