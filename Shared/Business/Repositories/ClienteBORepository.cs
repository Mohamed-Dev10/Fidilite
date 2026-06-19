using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.Contexts;
using BRICOMA.ECOMMERCE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Business.Repositories
{
    public class ClienteBORepository : IClienteBORepository
    {
        private readonly BRICOMAFIDELITEContext _context;

        public ClienteBORepository(BRICOMAFIDELITEContext context)
        {
            _context = context;
        }

        public async Task<RefCarteType?> GetRefCarteTypeById(int id)
        {
            return await _context.RefCarteType.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Cliente?> GetByCinAndType(string cin, int carteTypeId)
        {
            return await _context.Cliente
                .FirstOrDefaultAsync(f => f.Cin == cin
                    && f.RefCarteTypeId == carteTypeId
                    && f.IsActif != false);
        }

        public async Task<Cliente?> GetByGsmAndType(string gsm, int carteTypeId)
        {
            return await _context.Cliente
                .FirstOrDefaultAsync(f => f.Gsm == gsm
                    && f.RefCarteTypeId == carteTypeId
                    && f.IsActif != false);
        }

        public async Task<Cliente?> GetByEmailAndType(string email, int carteTypeId)
        {
            return await _context.Cliente
                .FirstOrDefaultAsync(f => f.Email == email
                    && f.RefCarteTypeId == carteTypeId
                    && f.IsActif != false);
        }

        public async Task<Cliente> Create(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> GetById(long id)
        {
            return await _context.Cliente
                .Include(i => i.RefMagasin)
                .Include(i => i.RefMetier)
                .Include(i => i.RefClienteStatut)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task Update(Cliente cliente)
        {
            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<RefMagasin?> GetMagasinById(int id)
        {
            return await _context.RefMagasin.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<RefVille?> GetVilleById(int id)
        {
            return await _context.RefVille.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<RefMetier?> GetMetierById(int id)
        {
            return await _context.RefMetier.FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Génère le prochain code client pour un magasin : MagasinCode + Alpha + 4 chiffres (ex: MAA0001).
        /// </summary>
        public async Task<string> GenerateClienteCode(int magasinId)
        {
            var magasin = await _context.RefMagasin.FirstOrDefaultAsync(m => m.Id == magasinId);
            if (magasin == null)
                throw new InvalidOperationException("Magasin introuvable.");

            var last = await _context.Cliente
                .Where(c => c.RefMagasinId == magasinId && c.Code != null)
                .OrderByDescending(c => c.Code)
                .FirstOrDefaultAsync();

            return last != null
                ? Extensions.ClienteExtensions.GetClienteCode(last.Code)
                : magasin.Code + "A0001";
        }

        /// <summary>
        /// Génère le prochain code-barres EAN-13 unique (séquentiel à partir de 6350000000109).
        /// </summary>
        public async Task<string> GenerateBarCode()
        {
            var last = await _context.Cliente
                .Where(c => c.CodeBarre != null && c.CodeBarre != "")
                .OrderByDescending(c => c.CodeBarre)
                .FirstOrDefaultAsync();

            long next = last != null && long.TryParse(last.CodeBarre, out var parsed)
                ? parsed + 1
                : Extensions.BarCodeExtensions.FirstBarCode;

            var barCode = Extensions.BarCodeExtensions.BuildEan13(next);

            while (await _context.Cliente.AnyAsync(c => c.CodeBarre == barCode))
            {
                next += 1;
                barCode = Extensions.BarCodeExtensions.BuildEan13(next);
            }

            return barCode;
        }

        public async Task<List<Cliente>> GetListByType(int carteTypeId, string? search, int? statutId, int? magasinId, int page, int pageSize)
        {
            var query = _context.Cliente
                .Include(c => c.RefMagasin)
                .Include(c => c.RefClienteStatut)
                .Where(c => c.RefCarteTypeId == carteTypeId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c =>
                    c.Nom.Contains(search) ||
                    c.Prenom.Contains(search) ||
                    c.Gsm.Contains(search) ||
                    (c.Cin != null && c.Cin.Contains(search)));

            if (statutId.HasValue)
                query = query.Where(c => c.RefClienteStatutId == statutId.Value);

            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);

            return await query
                .OrderByDescending(c => c.DateCreation)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByType(int carteTypeId, string? search, int? statutId, int? magasinId)
        {
            var query = _context.Cliente
                .Where(c => c.RefCarteTypeId == carteTypeId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c =>
                    c.Nom.Contains(search) ||
                    c.Prenom.Contains(search) ||
                    c.Gsm.Contains(search) ||
                    (c.Cin != null && c.Cin.Contains(search)));

            if (statutId.HasValue)
                query = query.Where(c => c.RefClienteStatutId == statutId.Value);

            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);

            return await query.CountAsync();
        }

        public async Task<int> CountTotal()
        {
            return await _context.Cliente.CountAsync();
        }

        public async Task<int> CountByCarteType(int carteTypeId)
        {
            return await _context.Cliente.CountAsync(c => c.RefCarteTypeId == carteTypeId);
        }

        public async Task<int> CountCreatedThisMonth()
        {
            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var end = start.AddMonths(1);
            return await _context.Cliente.CountAsync(c => c.DateCreation >= start && c.DateCreation < end);
        }

        public async Task<int> CountByActif(bool actif)
        {
            return actif
                ? await _context.Cliente.CountAsync(c => c.IsActif != false)
                : await _context.Cliente.CountAsync(c => c.IsActif == false);
        }

        public async Task<List<(string Magasin, int Count)>> CountGroupedByMagasin()
        {
            var grouped = await _context.Cliente
                .GroupBy(c => c.RefMagasin.Name)
                .Select(g => new { Magasin = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return grouped.Select(x => (x.Magasin ?? "-", x.Count)).ToList();
        }

        public async Task<List<RefMagasin>> GetAllMagasins()
        {
            return await _context.RefMagasin.OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<RefCarteType>> GetAllRefCarteTypes()
        {
            return await _context.RefCarteType.OrderBy(r => r.Name).ToListAsync();
        }

        public async Task<RefCarteType?> GetRefCarteTypeById2(int id)
        {
            return await _context.RefCarteType.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateRefCarteType(RefCarteType refCarteType)
        {
            _context.RefCarteType.Add(refCarteType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRefCarteType(RefCarteType refCarteType)
        {
            _context.RefCarteType.Update(refCarteType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRefCarteType(RefCarteType refCarteType)
        {
            _context.RefCarteType.Remove(refCarteType);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RefGenre>> GetAllGenres()
        {
            return await _context.RefGenre.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<List<RefMetier>> GetAllMetiers()
        {
            return await _context.RefMetier.OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<RefVille>> GetAllVilles()
        {
            return await _context.RefVille.OrderBy(v => v.Name).ToListAsync();
        }
    }
}
