using Restaurant.Data.Dtos;


namespace Restaurant.Service.Abstracts
{
    public interface IAuthService
    {
        Task<Response<string>> RegisterAsync(RegisterDto user);
        Task<Response<string>> RegisterAdminAsync(RegisterAdminDto Admin);
        Task<Response<AuthResponseDto>> ConfirmOtp(VerifyEmailOtpDTo model);
        Task<Response<string>> ResendOtp(ResentOtpDto model);
        Task<Response<AuthResponseDto>> LoginAsync(LogInDto user);
        Task<Response<string>> ForgetPasswordAsync(string emai);
        Task<Response<string>> ChangePasswordAsync(ChangePasswordDto model);
        Task<Response<string>> ResetPasswordAsync(ResetPasswordDto model);
        Task<Response<AuthResponseDto>> RefreshTokenAsync(RefreshRequestDto model);
    }
}
