using BRICOMA.ECOMMERCE.Models.Helpers;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IWhatsAppService
    {
        /// <summary>
        /// Envoie un message WhatsApp via Twilio.
        /// </summary>
        /// <param name="toGsm">GSM marocain au format local (ex: 0612345678).</param>
        /// <param name="body">Corps du message.</param>
        /// <param name="mediaUrl">URL optionnelle d'une image (ex: carte) à joindre.</param>
        Task<RESTServiceResponse<bool>> SendMessage(string toGsm, string body, string? mediaUrl = null);
    }
}
