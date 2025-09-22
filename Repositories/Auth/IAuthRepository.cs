using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<XRS_Users?> AuthenticateAdminUser(DTOs.LoginRequest request);
        
        string GenerateJwtAdminToken(XRS_Users user);

        Task<IActionResult> GenerateLoginOTPAsync(DTOs.LoginOTPDTO request);

        Task<IActionResult> GenerateForgotPasswordOTP(DTOs.ForgetPasswordOTPDTO request);

         Task<bool> ResetUserPassword(DTOs.ForegtPasswordDTO request);

    }
}
