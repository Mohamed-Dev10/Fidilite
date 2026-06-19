namespace BRICOMA.ECOMMERCE.Models.Helpers
{
    /// <summary>
    /// Résultat de la génération de l'image de carte.
    /// </summary>
    public class CardImageResult
    {
        /// <summary>
        /// Chemin web local de l'image générée (ex: "/cartes/MAA0001.png").
        /// Toujours renseigné après une génération réussie — sert à afficher/voir l'image
        /// dans le navigateur, même sans hébergement public.
        /// </summary>
        public string RelativeUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL publique absolue (ex: "https://.../cartes/MAA0001.png") pour Twilio WhatsApp.
        /// Null tant que PublicBaseUrl n'est pas configurée.
        /// </summary>
        public string? PublicUrl { get; set; }
    }
}
