using MediatR;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Commands.Model
{
    public class CompeleteReservationCommand : IRequest<Response<string>>
    {
        public int ReservationId { get; set; }

        public CompeleteReservationCommand(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
