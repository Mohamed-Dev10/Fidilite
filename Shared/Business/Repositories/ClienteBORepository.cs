using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.Contexts;
using BRICOMA.ECOMMERCE.Data.Models;
using BRICOMA.ECOMMERCE.Models.Enum;
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

        public async Task<int> CountTotal(int? magasinId = null)
        {
            // Le back-office ne compte que les cartes typées qu'il gère (M3alem, Artisan, types paramétrables) :
            // on exclut les cartes sans type (NULL, stock legacy) et les cartes AMIBRICOMA historiques.
            var query = _context.Cliente.Where(c => c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);
            return await query.CountAsync();
        }

        public async Task<int> CountByCarteType(int carteTypeId, int? magasinId = null)
        {
            var query = _context.Cliente.Where(c => c.RefCarteTypeId == carteTypeId);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);
            return await query.CountAsync();
        }

        public async Task<int> CountCreatedThisMonth(int? magasinId = null)
        {
            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var end = start.AddMonths(1);
            var query = _context.Cliente.Where(c => c.DateCreation >= start && c.DateCreation < end
                && c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);
            return await query.CountAsync();
        }

        public async Task<int> CountCreatedToday(int? magasinId = null)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var query = _context.Cliente.Where(c => c.DateCreation >= today && c.DateCreation < tomorrow
                && c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);
            return await query.CountAsync();
        }

        public async Task<int> CountCreatedInRange(DateTime from, DateTime to, int? magasinId = null)
        {
            var query = _context.Cliente.Where(c => c.DateCreation >= from && c.DateCreation < to
                && c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);
            return await query.CountAsync();
        }

        public async Task<int> CountByActif(bool actif, int? magasinId = null)
        {
            var query = actif
                ? _context.Cliente.Where(c => c.IsActif != false)
                : _context.Cliente.Where(c => c.IsActif == false);
            query = query.Where(c => c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);
            return await query.CountAsync();
        }

        public async Task<List<(string Magasin, int Count)>> CountGroupedByMagasin(int? magasinId = null)
        {
            var query = _context.Cliente.Where(c => c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);

            var grouped = await query
                .GroupBy(c => c.RefMagasin.Name)
                .Select(g => new { Magasin = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return grouped.Select(x => (x.Magasin ?? "-", x.Count)).ToList();
        }

        // Cartes gérées créées par jour sur les N derniers jours (pour la tendance du dashboard).
        // Mêmes exclusions que les autres agrégats : pas de type NULL, pas d'AMIBRICOMA.
        public async Task<List<(DateTime Day, int Count)>> CountGroupedByDay(int days, int? magasinId = null)
        {
            var start = DateTime.Today.AddDays(-(days - 1));
            var query = _context.Cliente.Where(c => c.DateCreation >= start
                && c.RefCarteTypeId != null && c.RefCarteTypeId != (int)CarteType.AMIBRICOMA);
            if (magasinId.HasValue)
                query = query.Where(c => c.RefMagasinId == magasinId.Value);

            var grouped = await query
                .GroupBy(c => c.DateCreation.Date)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .ToListAsync();

            return grouped.Select(x => (x.Day, x.Count)).ToList();
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
            // Le paramétrage référence le type (FK) : on le retire d'abord pour éviter
            // la violation de contrainte lors de la suppression du type.
            var parametrage = await _context.RefCarteTypeParametrage
                .FirstOrDefaultAsync(p => p.RefCarteTypeId == refCarteType.Id);
            if (parametrage != null)
                _context.RefCarteTypeParametrage.Remove(parametrage);

            _context.RefCarteType.Remove(refCarteType);
            await _context.SaveChangesAsync();
        }

        public async Task<RefCarteTypeParametrage?> GetParametrageByCarteTypeId(int carteTypeId)
        {
            return await _context.RefCarteTypeParametrage
                .FirstOrDefaultAsync(p => p.RefCarteTypeId == carteTypeId);
        }

        // Profil utilisateur (Nom/Prenom/magasin) : source officielle, liée 1:1 à AspNetUsers (Profil.Id = User.Id).
        public async Task<Profil?> GetProfilByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return await _context.Profil.FirstOrDefaultAsync(p => p.Id == userId);
        }

        public async Task UpsertProfil(string userId, string? nom, string? prenom, int? refMagasinId)
        {
            var existing = await _context.Profil.FirstOrDefaultAsync(p => p.Id == userId);
            if (existing == null)
            {
                _context.Profil.Add(new Profil
                {
                    Id = userId,
                    Nom = nom ?? string.Empty,
                    Prenom = prenom ?? string.Empty,
                    RefMagasinId = refMagasinId,
                    DateCreation = DateTime.Now
                });
            }
            else
            {
                existing.Nom = nom ?? string.Empty;
                existing.Prenom = prenom ?? string.Empty;
                existing.RefMagasinId = refMagasinId;
                _context.Profil.Update(existing);
            }
            await _context.SaveChangesAsync();
        }

        // Upsert : un seul paramétrage par type de carte (contrainte d'unicité en base).
        // removeImage : si true, l'image est explicitement retirée (ImagePath = null).
        public async Task SaveParametrage(RefCarteTypeParametrage parametrage, bool removeImage = false)
        {
            var existing = await _context.RefCarteTypeParametrage
                .FirstOrDefaultAsync(p => p.RefCarteTypeId == parametrage.RefCarteTypeId);

            if (existing == null)
            {
                if (removeImage) parametrage.ImagePath = null;
                _context.RefCarteTypeParametrage.Add(parametrage);
            }
            else
            {
                existing.MessageReception = parametrage.MessageReception;
                // Image : suppression explicite > nouvelle image > conservation de l'ancienne.
                if (removeImage)
                    existing.ImagePath = null;
                else if (!string.IsNullOrWhiteSpace(parametrage.ImagePath))
                    existing.ImagePath = parametrage.ImagePath;
                existing.BarcodeX = parametrage.BarcodeX;
                existing.BarcodeY = parametrage.BarcodeY;
                existing.BarcodeScale = parametrage.BarcodeScale;
                _context.RefCarteTypeParametrage.Update(existing);
            }

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
