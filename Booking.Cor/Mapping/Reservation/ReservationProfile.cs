using AutoMapper;

namespace Booking.Cor.Mapping.Reservation
{
    public partial class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            GetReservationForStaffMapping();
            GetReservationForUserMapping();
            // Command
            ApproveReservationMapping();
            CancelReservationMapping();
            CompeleteReservationMapping();
            CreateReservationMapping();
            RejectReservationMapping();
            UpdateReservationMapping();
        }
    }
}
