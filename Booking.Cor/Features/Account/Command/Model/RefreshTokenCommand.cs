using MediatR;
using Restaurant.Data.Dtos;
using Restaurant.Service;

namespace Booking.Cor.Features.Account.Command.Model
{
    public class RefreshTokenCommand : IRequest<Response<AuthResponseDto>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
