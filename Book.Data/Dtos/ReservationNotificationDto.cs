namespace Restaurant.Data.Dtos
{
    public class ReservationNotificationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartDateTime { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
