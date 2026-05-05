using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Account.Command.Model
{
    public class ForgetPasswordCommand : IRequest<Response<string>>
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
