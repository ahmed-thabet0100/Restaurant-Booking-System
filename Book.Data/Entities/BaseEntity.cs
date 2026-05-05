namespace Book.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CtetedAt { get; set; } = DateTime.UtcNow;
    }
}
