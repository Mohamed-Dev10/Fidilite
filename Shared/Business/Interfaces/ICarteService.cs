using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface ICarteService
    {
        Task<RESTServiceResponse<bool>> CreateM3alem(ClienteModel model);
        Task<RESTServiceResponse<bool>> CreateArtisan(ClienteModel model);
        Task<RESTServiceResponse<bool>> ConfirmationM3alem(string otpCode, long clienteId);
        Task<RESTServiceResponse<bool>> ConfirmationArtisan(string otpCode, long clienteId);
    }
}
