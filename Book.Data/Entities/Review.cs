using Book.Data.Entities;

namespace Booking.Data.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime CreatedAt = DateTime.UtcNow;

        public string UserId { get; set; }
        public string UserName { get; set; }
        public Book.Data.Entities.Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
    }
}
