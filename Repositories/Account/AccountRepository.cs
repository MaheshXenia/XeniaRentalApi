using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Account
{
    public class AccountRepository:IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.Accounts>> GetAccounts()
        {

            return await _context.Accounts.ToListAsync();

        }

        public async Task<IEnumerable<Models.Accounts>> GetAccountByCompanyId(int companyId)
        {

            return await _context.Accounts
                .Where(u => u.companyID == companyId)
                 .ToListAsync();

        }

        public async Task<IEnumerable<Models.Accounts>> GetAccountById(int accountId)
        {

            return await _context.Accounts
                .Where(u => u.accID == accountId)
                 .ToListAsync();

        }

        public async Task<Models.Accounts> CreateAccounts(Models.Accounts accounts)
        {

            await _context.Accounts.AddAsync(accounts);
            await _context.SaveChangesAsync();
            return accounts;

        }

        public async Task<bool> DeleteAccounts(int id)
        {
            var accountgroupsettings = await _context.Accounts.FirstOrDefaultAsync(u => u.accID == id);
            if (accountgroupsettings == null) return false;
           
            accountgroupsettings.modifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAccount(int id, Models.Accounts accounts)
        {
            var updatedAccounts = await _context.Accounts.FirstOrDefaultAsync(u => u.accID == id);
            if (updatedAccounts == null) return false;

            updatedAccounts.amountCr = accounts.amountCr;
            updatedAccounts.amountDr = accounts.amountDr;
            updatedAccounts.ledgerCr = accounts.ledgerCr;
            updatedAccounts.ledgerDr = accounts.ledgerDr;
            updatedAccounts.companyID = accounts.companyID;
            updatedAccounts.invNo = accounts.invNo;
            updatedAccounts.invType = accounts.invType;
            updatedAccounts.invDate = accounts.invDate;
            updatedAccounts.modifiedBy = accounts.modifiedBy;
            updatedAccounts.modifiedOn = accounts.modifiedOn;
            updatedAccounts.isActive = accounts.isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.Accounts>> GetAccountsAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.invType.Contains(search)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.invType) // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Accounts
                {
                    accID = u.accID,
                    amountCr = u.amountCr,
                    amountDr = u.amountDr,
                    invNo = u.invNo,
                    invType = u.invType,
                    invDate = u.invDate,
                    modifiedBy = u.modifiedBy,
                    modifiedOn = u.modifiedOn,
                    companyID = u.companyID,
                    VoucherId=u.VoucherId
                })
                .ToListAsync();

            return new PagedResultDto<Models.Accounts>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }


    }
}
