using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Repositories.Auth;
using XeniaRentalApi.Service.Common;


namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelperService _jwtHelperService;

        public AuthController(IAuthRepository authRepository, JwtHelperService jwtHelperService)
        {
            _authRepository = authRepository;
            _jwtHelperService = jwtHelperService;
        }

        #region ADMIN

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] DTOs.LoginRequest request)
        {
            try
            {
                var user = await _authRepository.AuthenticateAdminUser(request);

                if (user == null)
                {
                    return NotFound(new
                    {
                        Status = "Error",
                        Message = "User does not exist."
                    });
                }

                var token = _authRepository.GenerateJwtAdminToken(user);

                return Ok(new
                {
                    Status = "Success",
                    Message = "Login successful.",
                    Token = token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("OTP/login")]
        public async Task<IActionResult> GenerateLoginOTP([FromBody] DTOs.LoginOTPDTO request)
        {
            return await _authRepository.GenerateLoginOTPAsync(request);
        }

        [HttpPost]
        [Route("OTP/forgotPassword")]
        public async Task<IActionResult> GenerateForgotPasswordOTP([FromBody] DTOs.ForgetPasswordOTPDTO request)
        {
            return await _authRepository.GenerateForgotPasswordOTP(request);
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] DTOs.ForegtPasswordDTO request)
        {
            try
            {
                var result = await _authRepository.ResetUserPassword(request);
                if (!result)
                {
                    return Unauthorized(new { Status = "Error", Message = "Password reset faild." });
                }

                return Ok(new { Status = "Success", Message = "Password has been reset successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Status = "Error", Message = ex.Message });
            }
        }

        #endregion
    }
}
