using BRICOMA.ECOMMERCE.Models.Helpers;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface ICardImageService
    {
        /// <summary>
        /// Génère l'image de la carte (template + code-barres EAN-13 dessiné) et la sauvegarde
        /// dans wwwroot/cartes. Renvoie le chemin local (pour voir l'image) et l'URL publique
        /// (pour Twilio WhatsApp, null tant que PublicBaseUrl n'est pas configurée), ou null si
        /// la génération échoue.
        /// </summary>
        Task<CardImageResult?> GenerateCardImage(int carteTypeId, string clienteCode, string codeBarre);
    }
}
