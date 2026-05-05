using Microsoft.AspNetCore.Identity;

namespace Restaurant.Data.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Otp { get; set; }
        public string PhoneNumber { get; set; }
        public int? RestaurantId { get; set; }
        public DateTime? ExpiredOtp { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
