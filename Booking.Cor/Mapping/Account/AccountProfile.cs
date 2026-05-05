using AutoMapper;

namespace Restaurant.Cor.Mapping.Account
{
    public partial class AccountProfile : Profile
    {
        public AccountProfile()
        {
            RegisterCommandMapping();
            VerifyOTPCommandMapping();
            LogInCommandMapping();
            ResetPasswoedCommandMapping();
            ResetOtpCommandMapping();
            RegisterAdminCommandMapping();
            ChangePasswordCommandMapping();
            RefreshTokenCommandMapping();
        }
    }
}
