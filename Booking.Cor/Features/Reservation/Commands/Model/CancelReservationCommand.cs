using MediatR;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Commands.Model
{
    public class CancelReservationCommand : IRequest<Response<string>>
    {
        public CancelReservationCommand(int reservationId)
        {
            ReservationId = reservationId;
        }

        public int ReservationId { get; set; }
    }
}
