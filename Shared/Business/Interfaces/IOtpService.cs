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
        /// Supprime la demande OTP une fois la carte créée.
        /// </summary>
        Task Consume(string token);
    }
}
