using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<Users?> AuthenticateAdminUser(DTOs.LoginRequest request);
        
        string GenerateJwtAdminToken(Users user);

        Task<IActionResult> GenerateLoginOTPAsync(DTOs.LoginOTPDTO request);

        Task<IActionResult> GenerateForgotPasswordOTP(DTOs.ForgetPasswordOTPDTO request);

         Task<bool> ResetUserPassword(DTOs.ForegtPasswordDTO request);

    }
}
