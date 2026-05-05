using Booking.Cor.Features.Reservation.Queries.Result;
using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Booking.Cor.Features.Reservation.Commands.Model
{
    public class ApproveReservationCommand : IRequest<Response<GetReservationForStaff>>
    {
        public ApproveReservationCommand(int reservationId, int tableNumber)
        {
            ReservationId = reservationId;
            TableNumber = tableNumber;
        }

        [Required]
        public int ReservationId { get; set; }
        [Required]
        public int TableNumber { get; set; }
    }
}
