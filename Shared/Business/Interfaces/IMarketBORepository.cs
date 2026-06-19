using BRICOMA.ECOMMERCE.Data.Models.Market;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IMarketBORepository
    {
        Task<bool> CheckExisting(string gsm, string? cin, string? email);
        Task CreateClient(Client client);
        Task UpdateClient(Client client);
        Task<Client?> GetByCodeClt(string codeClt);
    }
}
