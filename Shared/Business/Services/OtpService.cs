using System.Security.Cryptography;
using System.Text.Json;
using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.Extensions.Logging;

namespace BRICOMA.ECOMMERCE.Business.Services
{
    public class OtpService : IOtpService
    {
        private const int ExpiryMinutes = 10;
        private const int MaxAttempts = 5;

        private readonly IOtpRepository _otpRepository;
        private readonly ILogger<OtpService> _logger;

        public OtpService(IOtpRepository otpRepository, ILogger<OtpService> logger)
        {
            _otpRepository = otpRepository;
            _logger = logger;
        }

        public async Task<(string token, string code)> Create(ClienteModel model)
        {
            var token = Guid.NewGuid().ToString("N");
            var code = GenerateCode();

          

            var otp = new OtpVerification
            {
                Token = token,
                Gsm = model.Gsm,
                Code = code,
                Payload = JsonSerializer.Serialize(model),
                Attempts = 0,
                IsUsed = false,
                ExpiresAt = DateTime.Now.AddMinutes(ExpiryMinutes),
                CreatedAt = DateTime.Now
            };

            await _otpRepository.Add(otp);

            _logger.LogInformation("OTP créé - Token: {Token}, GSM: {Gsm}", token, model.Gsm);

            return (token, code);
        }

        public async Task<RESTServiceResponse<ClienteModel>> Verify(string token, string code)
        {
            var otp = await _otpRepository.GetByToken(token);
            if (otp == null)
                return new RESTServiceResponse<ClienteModel>(false, "Demande introuvable ou expirée.", null);

            if (otp.IsUsed)
                return new RESTServiceResponse<ClienteModel>(false, "Cette demande a déjà été traitée.", null);

            if (otp.ExpiresAt < DateTime.Now)
                return new RESTServiceResponse<ClienteModel>(false, "Le code OTP a expiré. Veuillez recommencer.", null);

            if (otp.Attempts >= MaxAttempts)
                return new RESTServiceResponse<ClienteModel>(false, "Nombre de tentatives dépassé. Veuillez recommencer.", null);

            if (otp.Code != code?.Trim())
            {
                otp.Attempts += 1;
                await _otpRepository.Update(otp);
                var restant = MaxAttempts - otp.Attempts;
                return new RESTServiceResponse<ClienteModel>(false, $"Code OTP incorrect. {restant} tentative(s) restante(s).", null);
            }

            var model = JsonSerializer.Deserialize<ClienteModel>(otp.Payload);
            if (model == null)
                return new RESTServiceResponse<ClienteModel>(false, "Données de la demande illisibles.", null);

            return new RESTServiceResponse<ClienteModel>(true, "OTP vérifié.", model);
        }

        public async Task Consume(string token)
        {
            var otp = await _otpRepository.GetByToken(token);
            if (otp != null && !otp.IsUsed)
            {
                otp.IsUsed = true;
                await _otpRepository.Update(otp);
            }
        }

        private static string GenerateCode()
        {
            // Code à 6 chiffres
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        }
    }
}
