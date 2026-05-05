using System.ComponentModel.DataAnnotations;

namespace Restaurant.Data.Dtos
{
    public class ResentOtpDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

    }
}
