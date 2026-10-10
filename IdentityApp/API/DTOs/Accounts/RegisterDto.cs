using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class RegisterDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 15 characters.")]
        public string FirstName { get; set; } =string.Empty;
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 15 characters.")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 15 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
