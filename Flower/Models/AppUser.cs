using Microsoft.AspNetCore.Identity;

namespace Flower.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
