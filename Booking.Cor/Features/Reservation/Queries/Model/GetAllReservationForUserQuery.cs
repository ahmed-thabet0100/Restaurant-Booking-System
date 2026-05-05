using Booking.Cor.Features.Reservation.Queries.Result;
using Booking.Infra.Specifications.ReservationSpecification.User;
using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Queries.Model
{
    public class GetAllReservationForUserQuery : IRequest<Response<Pagination<GetReservationForUser>>>
    {
        public GetAllReservationForUserQuery(ReservationSpecParamsUser specParams)
        {
            SpecParams = specParams;
        }

        public ReservationSpecParamsUser SpecParams { get; set; }

    }
}
