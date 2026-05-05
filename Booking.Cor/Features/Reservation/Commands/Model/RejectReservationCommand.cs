using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Booking.Cor.Features.Reservation.Commands.Model
{
    public class RejectReservationCommand : IRequest<Response<string>>
    {
        public RejectReservationCommand(int reservationId)
        {
            ReservationId = reservationId;
        }

        [Required]
        public int ReservationId { get; set; }
    }
}
