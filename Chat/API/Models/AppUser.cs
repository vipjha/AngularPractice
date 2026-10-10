using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
    }
}
