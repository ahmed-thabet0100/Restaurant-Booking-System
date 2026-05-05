namespace Book.Data.Entities
{
    public class MenuItem : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}