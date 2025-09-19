using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Ledger
{
    public class LedgerRepository:ILedgerRepository
    {
        private readonly ApplicationDbContext _context;
        public LedgerRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.Ledger>> GetLedgers()
        {

            return await _context.Ledgers.ToListAsync();

        }


        public async Task<IEnumerable<Models.Ledger>> GetLedgerDetails(int companyId)
        {

            return await _context.Ledgers
                .Where(u => u.companyID == companyId)
                 .ToListAsync();

        }

        public async Task<IEnumerable<Models.Ledger>> GetLedgerbyId(int ledgerId)
        {

            return await _context.Ledgers
                .Where(u => u.ledgerID == ledgerId)
                 .ToListAsync();

        }

        public async Task<Models.Ledger> CreateLedger(Models.Ledger ledger)
        {

            await _context.Ledgers.AddAsync(ledger);
            await _context.SaveChangesAsync();
            return ledger;

        }

        public async Task<bool> UpdateLedger(int id, Models.Ledger ledger)
        {
            var UpdatedLedger = await _context.Ledgers.FirstOrDefaultAsync(u => u.ledgerID == id);
            if (UpdatedLedger == null) return false;

            UpdatedLedger.ledgerName = ledger.ledgerName ?? ledger.ledgerName;
            UpdatedLedger.companyID = ledger.companyID;
            UpdatedLedger.ledgerCode = ledger.ledgerCode;
            UpdatedLedger.groupID = ledger.groupID;
            UpdatedLedger.isActive = ledger.isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.Ledger>> GetLedgerAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Ledgers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.ledgerName.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.Ledger
                {
                    companyID = u.companyID,
                    ledgerID = u.ledgerID,
                    ledgerName = u.ledgerName,
                    isActive = u.isActive,
                    groupID = u.groupID,
                    ledgerCode = u.ledgerCode,


                })
                .ToListAsync();

            return new PagedResultDto<Models.Ledger>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
