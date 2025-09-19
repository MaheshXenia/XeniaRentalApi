using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Account
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Models.Accounts>> GetAccounts();
        Task<IEnumerable<Models.Accounts>> GetAccountByCompanyId(int companyId);

        Task<Models.Accounts> CreateAccounts(Models.Accounts account);

        Task<bool> DeleteAccounts(int id);

        Task<IEnumerable<Models.Accounts>> GetAccountById(int accountId);

        Task<bool> UpdateAccount(int id, Models.Accounts accounts);

        Task<PagedResultDto<Models.Accounts>> GetAccountsAsync(string? search, int pageNumber, int pageSize);

    }
}
