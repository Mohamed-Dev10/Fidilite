using Microsoft.AspNetCore.Identity;

namespace BRICOMA.ECOMMERCE.Data.ApplicationUser
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
