using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Ledger
{
    public interface ILedgerRepository
    {
        Task<IEnumerable<Models.Ledger>> GetLedgers();
        Task<IEnumerable<Models.Ledger>> GetLedgerDetails(int companyId);

        Task<Models.Ledger> CreateLedger(XeniaRentalApi.Models.Ledger ledger);
        Task<IEnumerable<Models.Ledger>> GetLedgerbyId(int ledgerId);

        Task<bool> UpdateLedger(int id, Models.Ledger ledger);

        Task<PagedResultDto<Models.Ledger>> GetLedgerAsync(string? search, int pageNumber, int pageSize);
    }
}
