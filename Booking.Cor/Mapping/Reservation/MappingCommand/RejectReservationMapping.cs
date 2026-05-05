using Booking.Cor.Features.Reservation.Commands.Model;

namespace Booking.Cor.Mapping.Reservation
{
    public partial class ReservationProfile
    {
        public void RejectReservationMapping()
        {
            CreateMap<RejectReservationCommand, Booking.Data.Entities.Reservation>().ReverseMap();
        }
    }
}
