namespace BRICOMA.ECOMMERCE.Models.Settings
{
    public class TwilioSettings
    {
        public const string SectionName = "Twilio";

        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;

        /// <summary>
        /// Numéro WhatsApp expéditeur Twilio, format "whatsapp:+14155238886" (sandbox free trial).
        /// </summary>
        public string WhatsAppFrom { get; set; } = string.Empty;
    }
}
