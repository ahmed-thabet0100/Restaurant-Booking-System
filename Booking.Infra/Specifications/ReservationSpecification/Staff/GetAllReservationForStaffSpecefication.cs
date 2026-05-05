using Booking.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.ReservationSpecification.Staff
{
    public class GetAllReservationForStaffSpecefication : BaseSpecification<Reservation>
    {
        public GetAllReservationForStaffSpecefication(ReservationSpecParamsStaff specParams, int RestaurantId)
            : base(r =>
                   r.RestaurantId == RestaurantId &&

                (specParams.Status == null || !specParams.Status.Any()
                    || specParams.Status.Contains(r.Status)) &&

                (!specParams.Date.HasValue ||
                (r.StartDateTime >= specParams.Date.Value.Date &&
                r.StartDateTime < specParams.Date.Value.Date.AddDays(1))) &&

                (!specParams.From.HasValue || r.StartDateTime >= specParams.From) &&
                (!specParams.To.HasValue || r.StartDateTime <= specParams.To)
            && (string.IsNullOrEmpty(specParams.UserId) || r.UserId == specParams.UserId)
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
