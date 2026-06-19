using BRICOMA.ECOMMERCE.Data.Models;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IClienteBOService
    {
        /// <summary>
        /// Étape 1 : valide, vérifie l'unicité (FIDELITE + MARKET), envoie l'OTP par WhatsApp.
        /// Renvoie le token de la demande OTP.
        /// </summary>
        Task<RESTServiceResponse<string>> InitCreate(ClienteModel model);

        /// <summary>
        /// Étape 2 : vérifie l'OTP puis crée la carte dans FIDELITE + MARKET (statut Confirmée),
        /// génère Code + CodeBarre EAN-13 et envoie la carte par WhatsApp. Une seule action.
        /// </summary>
        Task<RESTServiceResponse<string>> ConfirmCreate(string token, string otpCode);
        Task<RESTServiceResponse<PagedResult<Cliente>>> GetList(CarteListFilterModel filter);
        Task<RESTServiceResponse<DashboardStatsModel>> GetDashboardStats();
        Task<RESTServiceResponse<Cliente>> GetById(long id);
        Task<RESTServiceResponse<bool>> UpdateCarte(ClienteModel model);
        Task<RESTServiceResponse<List<RefMagasin>>> GetAllMagasins();
        Task<RESTServiceResponse<List<RefCarteType>>> GetAllRefCarteTypes();
        Task<RESTServiceResponse<bool>> CreateRefCarteType(string name);
        Task<RESTServiceResponse<bool>> UpdateRefCarteType(int id, string name);
        Task<RESTServiceResponse<bool>> DeleteRefCarteType(int id);
        Task<RESTServiceResponse<List<RefGenre>>> GetAllGenres();
        Task<RESTServiceResponse<List<RefMetier>>> GetAllMetiers();
        Task<RESTServiceResponse<List<RefVille>>> GetAllVilles();
    }
}
