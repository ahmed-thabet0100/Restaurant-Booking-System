namespace Booking.Cor.Features.Reservation.Queries.Result
{
    public class GetReservationForUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RestaurantName { get; set; }
        public int? TableNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int BookNumber { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
