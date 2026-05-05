using Booking.Cor.Features.Reservation.Queries.Result;
using MediatR;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Queries.Model
{
    public class GetReservationForStaffQuery : IRequest<Response<GetReservationForStaff>>
    {
        public GetReservationForStaffQuery(int reservationId)
        {
            ReservationId = reservationId;
        }

        public int ReservationId { get; set; }
    }
}
