using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.UserRole;

namespace XeniaRentalApi.Repositories.AccountGroups
{
    /// <summary>
    /// Account Group Repository for Adding,updating,creating and displaying accountGroups
    /// </summary>
    public class AccountGroupRepository: IAccountGroupRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountGroupRepository(ApplicationDbContext context) 
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.AccountGroups>> GetAccountGroups()
        {

            return await _context.AccountGroups.ToListAsync();

        }

        public async Task<IEnumerable<Models.AccountGroups>> GetAccountGroupsByCompanyId(int companyId)
        {

            return await _context.AccountGroups
                .Where(u => u.companyID == companyId)
                 .ToListAsync();

        }

        public async Task<IEnumerable<Models.AccountGroups>> GetAccountGroupById(int accountGroupId)
        {

            return await _context.AccountGroups
                .Where(u => u.groupID == accountGroupId)
                 .ToListAsync();

        }

        public async Task<Models.AccountGroups> CreateAccountGroups(Models.AccountGroups accountgroups)
        {

            await _context.AccountGroups.AddAsync(accountgroups);
            await _context.SaveChangesAsync();
            return accountgroups;

        }

        public async Task<bool> DeleteAccountGroup(int id)
        {
            var accountgroupsettings = await _context.AccountGroups.FirstOrDefaultAsync(u => u.groupID == id);
            if (accountgroupsettings == null) return false;
            accountgroupsettings.isActive = false;
            accountgroupsettings.modifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAccountGroup(int id, Models.AccountGroups updatedUser)
        {
            var updatedAccountGroup = await _context.AccountGroups.FirstOrDefaultAsync(u => u.groupID == id);
            if (updatedAccountGroup == null) return false;

            updatedAccountGroup.groupName = updatedAccountGroup.groupName ?? updatedAccountGroup.groupName;
            updatedAccountGroup.companyID = updatedAccountGroup.companyID;
            updatedAccountGroup.groupCode = updatedAccountGroup.groupCode;
            updatedAccountGroup.isActive = updatedAccountGroup.isActive;
            updatedAccountGroup.modifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.AccountGroups>> GetAccountGroupsAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.AccountGroups.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.groupName.Contains(search)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.groupName) // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.AccountGroups
                {
                    groupID = u.groupID,
                    groupName = u.groupName,
                    companyID = u.companyID,
                    groupCode = u.groupCode,
                    isActive = u.isActive,
                    modifiedOn = DateTime.UtcNow

                })
                .ToListAsync();

            return new PagedResultDto<Models.AccountGroups>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
