using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IOtpService
    {
        /// <summary>
        /// Génère un code OTP, persiste la demande (table OtpVerification) et renvoie (token, code).
        /// </summary>
        Task<(string token, string code)> Create(ClienteModel model);

        /// <summary>
        /// Vérifie le code pour un token donné. En cas de succès, renvoie le ClienteModel saisi.
        /// Gère expiration, code erroné et nombre de tentatives.
        /// </summary>
        Task<RESTServiceResponse<ClienteModel>> Verify(string token, string code);

        /// <summary>
        /// Régénère un nouveau code pour une demande existante (renvoi OTP) :
        /// réinitialise les tentatives, prolonge l'expiration et renvoie (GSM, nouveau code).
        /// </summary>
        Task<RESTServiceResponse<(string Gsm, string Code)>> Regenerate(string token);

        /// <summary>
        /// Supprime la demande OTP une fois la carte créée.
        /// </summary>
        Task Consume(string token);
    }
}
