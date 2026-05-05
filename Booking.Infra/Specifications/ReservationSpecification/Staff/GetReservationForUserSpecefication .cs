using Booking.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.ReservationSpecification.Staff
{
    public class GetReservationForUserSpecefication : BaseSpecification<Reservation>
    {
        public GetReservationForUserSpecefication(int ReservationId, string UserId)
            : base(r =>
                   r.Id == ReservationId && r.UserId == UserId

                 )
        {
            AddInclude(r => r.Table);
            AddInclude(r => r.Restaurant);

        }
    }
}
