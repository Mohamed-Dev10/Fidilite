using BRICOMA.ECOMMERCE.Data.Models;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IClienteBORepository
    {
        Task<RefCarteType?> GetRefCarteTypeById(int id);
        Task<Cliente?> GetByCinAndType(string cin, int carteTypeId);
        Task<Cliente?> GetByGsmAndType(string gsm, int carteTypeId);
        Task<Cliente?> GetByEmailAndType(string email, int carteTypeId);
        Task<Cliente> Create(Cliente cliente);
        Task<Cliente?> GetById(long id);
        Task Update(Cliente cliente);
        Task<RefMagasin?> GetMagasinById(int id);
        Task<RefVille?> GetVilleById(int id);
        Task<RefMetier?> GetMetierById(int id);
        Task<string> GenerateClienteCode(int magasinId);
        Task<string> GenerateBarCode();
        Task<List<Cliente>> GetListByType(int carteTypeId, string? search, int? statutId, int? magasinId, int page, int pageSize);
        Task<int> CountByType(int carteTypeId, string? search, int? statutId, int? magasinId);
        Task<int> CountTotal(int? magasinId = null);
        Task<int> CountByCarteType(int carteTypeId, int? magasinId = null);
        Task<int> CountCreatedThisMonth(int? magasinId = null);
        Task<int> CountCreatedToday(int? magasinId = null);
        Task<int> CountCreatedInRange(DateTime from, DateTime to, int? magasinId = null);
        Task<int> CountByActif(bool actif, int? magasinId = null);
        Task<(int Total, int Actives, int CreatedToday, int CreatedThisMonth, int CreatedThisWeek, int CreatedLastWeek)> GetDashboardCounts(int? magasinId = null);
        Task<Dictionary<int, int>> CountGroupedByCarteType(int? magasinId = null);
        Task<List<(string Magasin, int Count)>> CountGroupedByMagasin(int? magasinId = null);
        Task<List<(DateTime Day, int Count)>> CountGroupedByDay(int days, int? magasinId = null);
        Task<List<RefMagasin>> GetAllMagasins();
        Task<List<RefCarteType>> GetAllRefCarteTypes();
        Task<RefCarteType?> GetRefCarteTypeById2(int id);
        Task CreateRefCarteType(RefCarteType refCarteType);
        Task UpdateRefCarteType(RefCarteType refCarteType);
        Task DeleteRefCarteType(RefCarteType refCarteType);
        Task<RefCarteTypeParametrage?> GetParametrageByCarteTypeId(int carteTypeId);
        Task<Profil?> GetProfilByUserId(string userId);
        Task<List<Profil>> GetProfilsByUserIds(List<string> userIds);
        Task UpsertProfil(string userId, string? nom, string? prenom, int? refMagasinId);
        Task SaveParametrage(RefCarteTypeParametrage parametrage, bool removeImage = false);
        Task AddAuditLog(string userName, string operation, string entityType, string? entityCode, string? detail);
        Task<List<AuditLog>> GetAuditLogs(int page, int pageSize);
        Task<int> CountAuditLogs();
        Task<List<RefGenre>> GetAllGenres();
        Task<List<RefMetier>> GetAllMetiers();
        Task<List<RefVille>> GetAllVilles();
    }
}
