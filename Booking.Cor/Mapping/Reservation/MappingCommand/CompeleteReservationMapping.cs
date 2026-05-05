using Booking.Cor.Features.Reservation.Commands.Model;

namespace Booking.Cor.Mapping.Reservation
{
    public partial class ReservationProfile
    {
        public void CompeleteReservationMapping()
        {
            CreateMap<CompeleteReservationCommand, Booking.Data.Entities.Reservation>().ReverseMap();
        }
    }
}
