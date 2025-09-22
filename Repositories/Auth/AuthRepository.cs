﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XeniaCatalogueApi.Dictionary;
using XeniaRentalApi.Models;
using XeniaRentalApi.Service.Notification;

namespace XeniaRentalApi.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public AuthRepository(ApplicationDbContext context, IConfiguration configuration, INotificationService notificationService)
        {
            _context = context;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        #region ADMIN
        public async Task<XRS_Users?> AuthenticateAdminUser(DTOs.LoginRequest request)
        {
            var user = await _context.Users
                .Where(u => u.UserName == request.Username)
                .Select(u => new XRS_Users
                {
                    UserId = u.UserId,
                    CompanyId = u.CompanyId,
                    UserType = u.UserType,
                    UserName = u.UserName ?? string.Empty,
                    Password = u.Password ?? string.Empty,
                    IsActive = u.IsActive

                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            if (user.Password != request.Password)
            {
                throw new UnauthorizedAccessException("Incorrect password.");
            }
           
            return user;
        }

        public string GenerateJwtAdminToken(XRS_Users user)
        {
            var keyString = _configuration["JwtSettings:Key"]
                ?? throw new InvalidOperationException("JWT key is not configured.");

            var issuer = _configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException("JWT issuer is not configured.");

            var audience = _configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException("JWT audience is not configured.");

            var expirationMinutesString = _configuration["JwtSettings:ExpirationMinutes"]
                ?? throw new InvalidOperationException("JWT expiration is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("UserType", user.UserType.ToString() ?? "0"),
                    new Claim("CompanyId", user.CompanyId.ToString() ?? "0"),
                    new Claim(ClaimTypes.Role, user.UserType.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(expirationMinutesString)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ResetUserPassword(DTOs.ForegtPasswordDTO request)
        {
            var latestOtp = await _context.tblOTPLogs
                .Where(o => o.MobileNo == request.PhoneNumber && o.CompanyId ==request.CompanyId)
                .OrderByDescending(o => o.OTPId)
                .FirstOrDefaultAsync();

            if (latestOtp == null || latestOtp.OTP != request.OTP)
            {
                throw new UnauthorizedAccessException("Incorrect otp !");
            }

            if (latestOtp.ExpiryDate < DateTime.Now)
            {
                throw new UnauthorizedAccessException("otp was expired !");

            }


            var user = await _context.Users
                .Where(u => u.UserName == request.PhoneNumber && u.CompanyId == request.CompanyId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            user.Password = request.NewPassword;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IActionResult> GenerateLoginOTPAsync(DTOs.LoginOTPDTO request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == request.MobileNo && u.CompanyId == request.CompanyID);

            var isExistingUser = existingUser != null;

            var otpLog = new XRS_OTPLog
            {
                Type = isExistingUser ? (int)OTPType.LOGIN : (int)OTPType.REGISTRATION,
                MobileNo = request.MobileNo,
                CompanyId =request.CompanyID,
                OTP = GenerateOTP(),
                ExpiryDate = GetExpiryTime(request.CompanyID)
            };

            _context.tblOTPLogs.Add(otpLog);
            await _context.SaveChangesAsync();

            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "{otpcode}", otpLog.OTP },
                    { "{expiry}", GetExpiry(request.CompanyID) }
                };


            await _notificationService.SendNotification(
                request.CompanyID,
                null,
                NotificationType.REGISTRATION_OTP,
                request.MobileNo,
                request.Email,
                isExistingUser ? "LOGIN OTP" : "REGISTRATION OTP",
                parameters
            );

            return new OkObjectResult("OTP sent successfully.");
        }


        public async Task<IActionResult> GenerateForgotPasswordOTP(DTOs.ForgetPasswordOTPDTO request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == request.MobileNo && u.CompanyId == request.CompanyID);



            var otpLog = new XRS_OTPLog
            {
                Type = (int)OTPType.FORGOT_PASSWORD,
                MobileNo = request.MobileNo,
                CompanyId = request.CompanyID,
                OTP = GenerateOTP(),
                ExpiryDate = GetExpiryTime(request.CompanyID)
            };

            _context.tblOTPLogs.Add(otpLog);
            await _context.SaveChangesAsync();

            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "{otpcode}", otpLog.OTP },
                    { "{expiry}", GetExpiry(request.CompanyID)}
                };


            /*await _notificationService.SendNotification(
                companyId,
                null,
                NotificationType.FORGOT_OTP,
                mobileNo,
                email,
                "FORGOT_PASSWORD",
                parameters
            );*/

            return new OkObjectResult("OTP sent successfully.");
        }

        private string GenerateOTP()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private DateTime GetExpiryTime(int companyId)
        {
            return DateTime.Now.AddMinutes(10);
        }

        private string GetExpiry(int companyId)
        {
            return "10 minutes";
        }

        #endregion
    }
}
