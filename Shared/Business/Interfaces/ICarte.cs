using BRICOMA.ECOMMERCE.Models.Models;
using BRICOMA.ECOMMERCE.Models.Helpers;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface ICarte
    {
        Task<RESTServiceResponse<bool>> CreateM3alem(ClienteModel model);
        Task<RESTServiceResponse<bool>> CreateArtisan(ClienteModel model);
        Task<RESTServiceResponse<bool>> ConfirmationM3alem(string otpCode, long clienteId);
        Task<RESTServiceResponse<bool>> ConfirmationArtisan(string otpCode, long clienteId);
    }
}
