using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.UserRole
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetUsers();
        Task<IEnumerable<UserRoles>> GetUserRoles();

        Task<IEnumerable<Models.Users>> GetUserByCompanyId(int companyId);
        Task<IEnumerable<Models.Users>> GetUserById(int Id);

        Task<Models.Users> CreateUser(DTOs.CreateUser userSettings);
        Task<bool> UpdateUserSetting(int id, Users userSettings);
        Task<bool> DeleteUserSetting(int id);

    }
}
