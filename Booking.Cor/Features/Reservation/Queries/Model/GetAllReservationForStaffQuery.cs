using Booking.Cor.Features.Reservation.Queries.Result;
using Booking.Infra.Specifications.ReservationSpecification;
using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Infra.Specifications.RestaurantSpecifications;
using Restaurant.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Cor.Features.Reservation.Queries.Model
{
    public class GetAllReservationForStaffQuery : IRequest<Response<Pagination<GetReservationForStaff>>>
    {
        public GetAllReservationForStaffQuery(ReservationSpecParamsStaff specParams)
        {
            SpecParams = specParams;
        }

        public ReservationSpecParamsStaff SpecParams { get; set; }

    }
}
