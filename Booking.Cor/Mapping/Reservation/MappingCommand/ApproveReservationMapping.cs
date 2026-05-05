using Booking.Cor.Features.Reservation.Commands.Model;

namespace Booking.Cor.Mapping.Reservation
{
    public partial class ReservationProfile
    {
        public void ApproveReservationMapping()
        {
            CreateMap<ApproveReservationCommand, Booking.Data.Entities.Reservation>().ReverseMap();
        }
    }
}
