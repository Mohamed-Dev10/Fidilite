namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class UpdateUserModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? NewPassword { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public int? RefMagasinId { get; set; }
        public bool IsActif { get; set; }
    }
}
