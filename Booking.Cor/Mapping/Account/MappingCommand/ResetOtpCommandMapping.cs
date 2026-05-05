using Restaurant.Cor.Features.Account.Command.Model;
using Restaurant.Data.Dtos;

namespace Restaurant.Cor.Mapping.Account
{
    public partial class AccountProfile
    {
        public void ResetOtpCommandMapping()
        {
            CreateMap<ResentOtpCommand, ResentOtpDto>().ReverseMap();
        }
    }
}
