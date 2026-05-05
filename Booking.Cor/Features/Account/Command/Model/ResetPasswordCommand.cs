using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Account.Command.Model
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

    }
}
