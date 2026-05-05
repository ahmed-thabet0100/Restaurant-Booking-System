using Restaurant.Data.Dtos;

namespace Booking.Infra.Specifications.ReservationSpecification.User
{
    public class ReservationSpecParamsUser
    {
        public List<ReservationStatus>? Status { get; set; }
        public int? RestaurantId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

    }
}
