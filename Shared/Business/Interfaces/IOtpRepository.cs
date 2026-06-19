using BRICOMA.ECOMMERCE.Data.ApplicationUser;

namespace BRICOMA.ECOMMERCE.Business.Interfaces
{
    public interface IOtpRepository
    {
        Task<OtpVerification> Add(OtpVerification otp);
        Task<OtpVerification?> GetByToken(string token);
        Task Update(OtpVerification otp);
        Task Delete(OtpVerification otp);
    }
}
