namespace BRICOMA.ECOMMERCE.Models.Settings
{
    public class MediaSettings
    {
        public const string SectionName = "Media";

        /// <summary>
        /// URL publique de base (sans slash final) servant à construire l'URL des cartes
        /// pour Twilio WhatsApp (ex: "https://carte.bricoma.ma"). Vide tant que l'hébergement
        /// public n'est pas branché → l'image est générée mais pas envoyée par WhatsApp.
        /// </summary>
        public string PublicBaseUrl { get; set; } = string.Empty;
    }
}
