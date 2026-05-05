using Booking.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.ReservationSpecification.User
{
    public class GetAllReservationForUserSpecification : BaseSpecification<Reservation>
    {
        public GetAllReservationForUserSpecification(ReservationSpecParamsUser specParams, string UserId)
            : base(r =>
                r.UserId == UserId &&

                (specParams.Status == null || !specParams.Status.Any()
                    || specParams.Status.Contains(r.Status)) &&

                (!specParams.Date.HasValue ||
                    r.StartDateTime >= specParams.Date.Value.Date &&
                     r.StartDateTime < specParams.Date.Value.Date.AddDays(1)) &&

                (!specParams.From.HasValue || r.StartDateTime >= specParams.From) &&
                (!specParams.To.HasValue || r.StartDateTime <= specParams.To)

            && (!specParams.RestaurantId.HasValue || r.RestaurantId == specParams.RestaurantId)
            )
        {
            AddInclude(r => r.Table);
            AddInclude(r => r.Restaurant);

            AddOrderByDescending(r => r.StartDateTime);

            ApplyPaging(
                specParams.PageSize * (specParams.PageIndex - 1),
                specParams.PageSize);
        }
    }
}
