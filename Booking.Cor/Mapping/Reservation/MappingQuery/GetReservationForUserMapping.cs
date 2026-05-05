using Booking.Cor.Features.Reservation.Queries.Result;
using Booking.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Cor.Mapping.Reservation
{
    public partial class ReservationProfile
    {
        public void GetReservationForUserMapping()
        {
            CreateMap<GetReservationForUser, Booking.Data.Entities.Reservation>().ReverseMap();
        }
    }
}
