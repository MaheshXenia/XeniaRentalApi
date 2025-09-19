using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.AccountGroups
{
    public interface IAccountGroupRepository
    {
        Task<IEnumerable<Models.AccountGroups>> GetAccountGroups();
        Task<IEnumerable<Models.AccountGroups>> GetAccountGroupsByCompanyId(int companyId);

        Task<IEnumerable<Models.AccountGroups>> GetAccountGroupById(int accountGroupId);

        Task<Models.AccountGroups> CreateAccountGroups(XeniaRentalApi.Models.AccountGroups account);

        Task<bool> DeleteAccountGroup(int id);

        Task<bool> UpdateAccountGroup(int id, Models.AccountGroups updatedUser);
        Task<PagedResultDto<Models.AccountGroups>> GetAccountGroupsAsync(string? search, int pageNumber, int pageSize);
    }
}
