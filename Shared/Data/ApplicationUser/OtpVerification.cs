namespace BRICOMA.ECOMMERCE.Data.ApplicationUser
{
    public class OtpVerification
    {
        public long Id { get; set; }

        /// <summary>
        /// Jeton unique transmis au formulaire OTP pour retrouver la demande.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        public string Gsm { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// JSON du ClienteModel saisi, conservé côté serveur jusqu'à la confirmation.
        /// </summary>
        public string Payload { get; set; } = string.Empty;

        public int Attempts { get; set; }

        /// <summary>
        /// Marque l'OTP comme consommé après création réussie de la carte (la ligne est conservée).
        /// </summary>
        public bool IsUsed { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
