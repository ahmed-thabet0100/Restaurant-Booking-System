using Booking.Data.Entities;

namespace Book.Data.Entities
{
    public class Restaurant : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public string OpeningHours { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public bool NeedsRatingUpdate { get; set; }
        public string OwnerId { get; set; }
        public int BookTotal { get; set; } = 0;

        public List<Review>? Reviews { get; set; }
        public ICollection<Table> Tables { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
