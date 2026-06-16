using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.Contexts;
using BRICOMA.ECOMMERCE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Business.Repositories
{
    public class CarteRepository : ICarteRepository
    {
        private readonly BRICOMAFIDELITEContext _context;

        public CarteRepository(BRICOMAFIDELITEContext context)
        {
            _context = context;
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

        public async Task<Cliente> Create(Cliente carte)
        {
            _context.Cliente.Add(carte);
            await _context.SaveChangesAsync();
            return carte;
        }

        public async Task<Cliente?> GetById(long id)
        {
            return await _context.Cliente
                .Include(i => i.RefMagasin)
                .Include(i => i.RefMetier)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task Update(Cliente carte)
        {
            _context.Cliente.Update(carte);
            await _context.SaveChangesAsync();
        }
    }
}
