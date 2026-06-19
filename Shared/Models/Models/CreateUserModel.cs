namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class CreateUserModel
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public int? RefMagasinId { get; set; }
    }
}
