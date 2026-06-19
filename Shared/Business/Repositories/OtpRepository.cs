using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Business.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OtpVerification> Add(OtpVerification otp)
        {
            _context.OtpVerifications.Add(otp);
            await _context.SaveChangesAsync();
            return otp;
        }

        public async Task<OtpVerification?> GetByToken(string token)
        {
            return await _context.OtpVerifications.FirstOrDefaultAsync(o => o.Token == token);
        }

        public async Task Update(OtpVerification otp)
        {
            _context.OtpVerifications.Update(otp);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(OtpVerification otp)
        {
            _context.OtpVerifications.Remove(otp);
            await _context.SaveChangesAsync();
        }
    }
}
