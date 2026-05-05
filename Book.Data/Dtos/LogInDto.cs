using System.ComponentModel.DataAnnotations;

namespace Restaurant.Data.Dtos
{
    public class LogInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
