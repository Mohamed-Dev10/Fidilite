using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BRICOMA.ECOMMERCE.Business.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly TwilioSettings _settings;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(IOptions<TwilioSettings> settings, ILogger<WhatsAppService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<RESTServiceResponse<bool>> SendMessage(string toGsm, string body, string? mediaUrl = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_settings.AccountSid) || string.IsNullOrWhiteSpace(_settings.AuthToken))
                    return new RESTServiceResponse<bool>(false, "Configuration Twilio manquante.", false);

                var to = ToWhatsAppAddress(toGsm);
                if (to == null)
                    return new RESTServiceResponse<bool>(false, "Numéro GSM invalide.", false);

                TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

                var options = new CreateMessageOptions(new PhoneNumber(to))
                {
                    From = new PhoneNumber(_settings.WhatsAppFrom),
                    Body = body
                };

                if (!string.IsNullOrWhiteSpace(mediaUrl))
                    options.MediaUrl = new List<Uri> { new Uri(mediaUrl) };

                var message = await MessageResource.CreateAsync(options);

                _logger.LogInformation("WhatsApp envoyé - To: {To}, Sid: {Sid}, Status: {Status}", to, message.Sid, message.Status);

                return new RESTServiceResponse<bool>(true, "Message envoyé.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur envoi WhatsApp - GSM: {Gsm}", toGsm);
                return new RESTServiceResponse<bool>(false, $"Échec de l'envoi WhatsApp : {ex.Message}", false);
            }
        }

        /// <summary>
        /// Convertit un GSM marocain local (0612345678) au format WhatsApp international (whatsapp:+212612345678).
        /// </summary>
        private static string? ToWhatsAppAddress(string gsm)
        {
            if (string.IsNullOrWhiteSpace(gsm))
                return null;

            var digits = new string(gsm.Where(char.IsDigit).ToArray());

            // 0612345678 -> 212612345678
            if (digits.Length == 10 && digits.StartsWith("0"))
                digits = "212" + digits.Substring(1);
            // 212612345678 déjà international
            else if (digits.Length == 12 && digits.StartsWith("212"))
            {
                // ok
            }
            else
            {
                return null;
            }

            return $"whatsapp:+{digits}";
        }
    }
}
