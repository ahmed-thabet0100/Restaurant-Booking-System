using System.ComponentModel.DataAnnotations;

namespace Restaurant.Data.Dtos
{
    public class RegisterAdminDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "⚠️ Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string? Code { get; set; } = null;

    }
}
