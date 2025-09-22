using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Ledger
{
    public interface IAccountLedgerRepository
    {
        Task<IEnumerable<XRS_AccountLedger>> GetLedgers();

        Task<IEnumerable<XRS_AccountLedger>> GetLedgerDetails(int companyId);

        Task<IEnumerable<XRS_AccountLedger>> GetLedgerbyId(int ledgerId);

        Task<Models.XRS_AccountLedger> CreateLedger(XRS_AccountLedger ledger);
   
        Task<bool> UpdateLedger(int id, XRS_AccountLedger ledger);
       
    }
}
