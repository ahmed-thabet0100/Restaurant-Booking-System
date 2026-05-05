namespace Restaurant.Data.Entities
{
    public class RestaurantAdminInvitation
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int RestaurantId { get; set; }

        public string Code { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }
    }
}
