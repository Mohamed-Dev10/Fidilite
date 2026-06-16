using BRICOMA.ECOMMERCE.Data.Models;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface ICarteRepository
    {
        // Vérifications doublons
        Task<Cliente?> GetByCinAndType(string cin, int carteTypeId);
        Task<Cliente?> GetByGsmAndType(string gsm, int carteTypeId);
        Task<Cliente?> GetByEmailAndType(string email, int carteTypeId);

        // CRUD
        Task<Cliente> Create(Cliente carte);
        Task<Cliente?> GetById(long id);
        Task Update(Cliente carte);
    }
}
