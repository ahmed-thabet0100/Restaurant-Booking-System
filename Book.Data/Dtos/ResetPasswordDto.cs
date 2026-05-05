using System.ComponentModel.DataAnnotations;

namespace Restaurant.Data.Dtos
{
    public class ResetPasswordDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

    }
}
