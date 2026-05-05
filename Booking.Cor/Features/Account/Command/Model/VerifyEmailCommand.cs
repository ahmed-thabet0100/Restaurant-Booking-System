using MediatR;
using Restaurant.Data.Dtos;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Account.Command.Model
{
    public class VerifyEmailCommand : IRequest<Response<AuthResponseDto>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; set; }
    }
}
