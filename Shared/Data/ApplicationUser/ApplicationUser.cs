using Microsoft.AspNetCore.Identity;

namespace BRICOMA.ECOMMERCE.Data.ApplicationUser
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public int? RefMagasinId { get; set; }
    }
}
