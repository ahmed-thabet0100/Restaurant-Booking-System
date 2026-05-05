using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Account.Command.Model
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}
