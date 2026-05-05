using Restaurant.Data.Dtos;

namespace Booking.Infra.Specifications.ReservationSpecification
{
    public class ReservationSpecParamsStaff
    {
        public List<ReservationStatus>? Status { get; set; }
        public string? UserId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

    }
}
