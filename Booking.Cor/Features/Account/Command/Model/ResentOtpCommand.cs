using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Account.Command.Model
{
    public class ResentOtpCommand : IRequest<Response<string>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
