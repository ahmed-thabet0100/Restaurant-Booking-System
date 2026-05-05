namespace Book.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }
    }
}