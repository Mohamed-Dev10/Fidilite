using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.Contexts;
using BRICOMA.ECOMMERCE.Data.Models.Market;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Business.Repositories
{
    public class MarketBORepository : IMarketBORepository
    {
        private readonly BRICOMAMARKETContext _context;

        public MarketBORepository(BRICOMAMARKETContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExisting(string gsm, string? cin, string? email)
        {
            var query = _context.Clients.AsQueryable();

            // GSM toujours vérifié
            var byGsm = await query.AnyAsync(c => c.Telmob == gsm && c.Actif != false);
            if (byGsm) return true;

            // CIN si fourni
            if (!string.IsNullOrWhiteSpace(cin))
            {
                var byCin = await query.AnyAsync(c => c.Cin == cin && c.Actif != false);
                if (byCin) return true;
            }

            // Email si fourni
            if (!string.IsNullOrWhiteSpace(email))
            {
                var byEmail = await query.AnyAsync(c => c.Email == email && c.Actif != false);
                if (byEmail) return true;
            }

            return false;
        }

        public async Task CreateClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClient(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task<Client?> GetByCodeClt(string codeClt)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.CodeClt == codeClt);
        }
    }
}
