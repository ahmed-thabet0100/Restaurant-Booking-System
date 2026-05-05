using Hangfire;
using Microsoft.AspNetCore.Identity;
using Restaurant.Data.Dtos;
using Restaurant.Data.Entities.Identity;
using Restaurant.Infra.Abstracts;
using Restaurant.Service.Abstracts;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Restaurant.Service.Implementation
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokernService _createToken;
        private readonly Abstracts.IEmailSender _emailSender;
        private readonly IInvetationRepo _invetationRepo;
        private readonly ResponseHandler _responseHandler;
        private readonly ICurrentUserService _currentUser;

        public AuthService(
            UserManager<AppUser> userManager,
            ITokernService CreateToken,
            IEmailSender emailSender,
            IInvetationAdminSender invetationAdminSender,
            IInvetationRepo invetationRepo,
            ResponseHandler responseHandler,
            ICurrentUserService currentUser
            )
        {
            _userManager = userManager;
            _createToken = CreateToken;
            _emailSender = emailSender;
            _invetationRepo = invetationRepo;
            _responseHandler = responseHandler;
            _currentUser = currentUser;
        }
        public async Task<Response<string>> RegisterAsync(RegisterDto model)
        {
            var isExist = await _userManager.FindByEmailAsync(model.Email);
            if (isExist != null)
                return _responseHandler.Conflict<string>("Email already exists.");

            if (model.Password != model.ConfirmPassword)
                return _responseHandler.BadRequest<string>("Password confirmation does not match.");

            var allowedRoles = new List<string> { "User", "Manager" };

            var selectedRole = "User";

            if (!string.IsNullOrWhiteSpace(model.Role))
            {
                if (model.Role.ToLower() == "manager")
                    selectedRole = "Manager";
                else if (model.Role.ToLower() == "user")
                    selectedRole = "User";
                else
                    return _responseHandler.BadRequest<string>("Invalid role selection.");
            }

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email.ToLower().Trim(),
                UserName = model.Email.ToLower().Trim(),
                Otp = otp,
                ExpiredOtp = DateTime.UtcNow.AddMinutes(5)
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return _responseHandler.BadRequest<string>(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            await _userManager.AddToRoleAsync(user, selectedRole);

            _emailSender.SendEmailAsync(user.Email,
                "Email Verifaction Code",
                $"Your verification code is <b>{otp}</b><br/>Valid for 5 minutes."
                );
            //BackgroundJob.Enqueue<IEmailSender>(x => x.SendEmailAsync(
            //    user.Email,
            //    "Email Verification Code",
            //    $"Your verification code is <b>{otp}</b><br/>Valid for 5 minutes."
            //));

            return _responseHandler.Success<string>("OTP sent to your email");
        }
        public async Task<Response<AuthResponseDto>> ConfirmOtp(VerifyEmailOtpDTo model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return _responseHandler.NotFound<AuthResponseDto>("User Not Found");

            if (user.EmailConfirmed)
                return _responseHandler.Unauthorized<AuthResponseDto>("Email Already Confirmed");

            if (model.Otp != user.Otp)
                return _responseHandler.BadRequest<AuthResponseDto>("Invalid or expired OTP");

            if (user.ExpiredOtp == null || DateTime.UtcNow > user.ExpiredOtp)
                return _responseHandler.BadRequest<AuthResponseDto>("OTP has expired");

            user.EmailConfirmed = true;
            user.Otp = null;
            user.ExpiredOtp = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return _responseHandler.BadRequest<AuthResponseDto>(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            var token = await _createToken.CreateToken(user, _userManager);
            var refreshtoken = _createToken.GenerateRefreshToken();

            user.RefreshToken = _createToken.HashToken(refreshtoken);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return _responseHandler.Success<AuthResponseDto>(
                new AuthResponseDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = token,
                    RefreshToken = refreshtoken
                },
                "Email Confirmed Successfully"
            );
        }
        public async Task<Response<string>> ResendOtp(ResentOtpDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return _responseHandler.BadRequest<string>("Invalid email");

            if (user.EmailConfirmed)
                return _responseHandler.Conflict<string>("Email already confirmed");

            // منع إرسال OTP جديد قبل انتهاء القديم
            if (user.ExpiredOtp.HasValue && user.ExpiredOtp > DateTime.UtcNow)
                return _responseHandler.BadRequest<string>("Please wait until the current OTP expires.");

            // توليد OTP آمن
            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            user.Otp = otp;
            user.ExpiredOtp = DateTime.UtcNow.AddMinutes(5);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return _responseHandler.BadRequest<string>(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            BackgroundJob.Enqueue<IEmailSender>(x => x.SendEmailAsync
            (
                user.Email,
                "Email Verification Code",
                $"Your verification code is <b>{otp}</b><br/>Valid for 5 minutes."
            ));

            return _responseHandler.Success<string>("OTP sent to your email");
        }
        public async Task<Response<AuthResponseDto>> LoginAsync(LogInDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return _responseHandler.Unauthorized<AuthResponseDto>("Invalid email or password");
            if (!user.EmailConfirmed)
                return _responseHandler.BadRequest<AuthResponseDto>("Please confirm your email first");
            var check = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!check)
                return _responseHandler.Unauthorized<AuthResponseDto>("Invalid email or password");

            var accessToken = await _createToken.CreateToken(user, _userManager);
            var refreshToken = _createToken.GenerateRefreshToken();

            user.RefreshToken = _createToken.HashToken(refreshToken);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return _responseHandler.Success(new AuthResponseDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = accessToken,
                RefreshToken = refreshToken
            }, "Login successful");
        }
        public async Task<Response<string>> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetLink = $"https://yourfrontend/reset-password?email={email}&token={token}";

                BackgroundJob.Enqueue<IEmailSender>(x => x.SendEmailAsync(
                    email,
                    "Reset Password",
                    $"Click here to reset password: <a href='{resetLink}'>Reset Password</a>"
                ));
            }

            return _responseHandler.Success<string>(
                "If the email exists, a reset link has been sent."
            );
        }
        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return _responseHandler.BadRequest<string>("Invalid request");

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                return _responseHandler.BadRequest<string>(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }

            return _responseHandler.Success<string>("Password updated successfully");
        }

        public async Task<Response<string>> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);

            if (user == null)
                return _responseHandler.Unauthorized<string>("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                return _responseHandler.BadRequest<string>(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }

            return _responseHandler.Success<string>("Password changed successfully");
        }
        public async Task<Response<string>> RegisterAdminAsync(RegisterAdminDto model)
        {
            var invite = await _invetationRepo.GetByCodeAsync(model.Code);

            if (invite == null || model.Code != invite.Code)
                return _responseHandler.BadRequest<string>("Invalid code");

            if (invite.IsUsed)
                return _responseHandler.Conflict<string>("Invitation already used");

            if (invite.ExpiryDate < DateTime.UtcNow)
                return _responseHandler.BadRequest<string>("Invitation expired");

            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email,
                RestaurantId = invite.RestaurantId,
                EmailConfirmed = true
            };

            // 1. Create User
            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
                return _responseHandler.BadRequest<string>(
                    string.Join(", ", createResult.Errors.Select(e => e.Description))
                );

            // 2. Add To Role
            var roleResult = await _userManager.AddToRoleAsync(user, "Staff");

            if (!roleResult.Succeeded)
                return _responseHandler.BadRequest<string>(
                    string.Join(", ", roleResult.Errors.Select(e => e.Description))
                );

            // 3. Update Invitation
            invite.IsUsed = true;
            await _invetationRepo.UpdateAsync(invite);

            return _responseHandler.Success<string>("Admin created successfully");
        }

        public async Task<Response<AuthResponseDto>> RefreshTokenAsync(RefreshRequestDto model)
        {
            var principal = _createToken.GetPrincipalFromExpiredToken(model.AccessToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null ||
                user.RefreshToken != _createToken.HashToken(model.RefreshToken) ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return _responseHandler.Unauthorized<AuthResponseDto>("Invalid refresh token");
            }

            // 🔥 generate new tokens
            var newAccessToken = await _createToken.CreateToken(user, _userManager);
            var newRefreshToken = _createToken.GenerateRefreshToken();

            user.RefreshToken = _createToken.HashToken(newRefreshToken);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return _responseHandler.Success(new AuthResponseDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

    }
}
