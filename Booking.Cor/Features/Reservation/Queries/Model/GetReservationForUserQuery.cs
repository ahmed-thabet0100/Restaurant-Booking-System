using Booking.Cor.Features.Reservation.Queries.Result;
using MediatR;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Queries.Model
{
    public class GetReservationForUserQuery : IRequest<Response<GetReservationForUser>>
    {
        public GetReservationForUserQuery(int reservationId)
        {
            ReservationId = reservationId;
        }

        public int ReservationId { get; set; }

    }
}
