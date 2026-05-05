using Booking.Cor.Features.Reservation.Queries.Result;

namespace Booking.Cor.Mapping.Reservation
{
    public partial class ReservationProfile
    {
        public void GetReservationForStaffMapping()
        {
            CreateMap<GetReservationForStaff, Booking.Data.Entities.Reservation>().ReverseMap();
        }
    }
}
