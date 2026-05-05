using Booking.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.ReservationSpecification.Staff
{
    public class GetReservationForStaffSpecefication : BaseSpecification<Reservation>
    {
        public GetReservationForStaffSpecefication(int ReservationId, int RestaurantId)
            : base(r =>
                   r.RestaurantId == RestaurantId &&
            r.Id == ReservationId)
        {
            AddInclude(r => r.Table);
            AddInclude(r => r.Restaurant);

        }
    }
}
