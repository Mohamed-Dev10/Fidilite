using System.Drawing;
using System.Drawing.Imaging;
using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Models.Enum;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Settings;
using Microsoft.Extensions.Options;
using Spire.Barcode;

namespace BRICOMA.ECOMMERCE.Web.Services
{
    public class CardImageService : ICardImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly MediaSettings _media;
        private readonly ILogger<CardImageService> _logger;

        public CardImageService(IWebHostEnvironment env, IOptions<MediaSettings> media, ILogger<CardImageService> logger)
        {
            _env = env;
            _media = media.Value;
            _logger = logger;
        }

        public async Task<CardImageResult?> GenerateCardImage(int carteTypeId, string clienteCode, string codeBarre)
        {
            try
            {
                var (templateFile, x, y) = GetTemplate(carteTypeId);
                var templatePath = Path.Combine(_env.WebRootPath, "media", templateFile);

                if (!File.Exists(templatePath))
                {
                    _logger.LogError("Template carte introuvable : {Path}", templatePath);
                    return null;
                }

                var outputDir = Path.Combine(_env.WebRootPath, "cartes");
                Directory.CreateDirectory(outputDir);
                var outputPath = Path.Combine(outputDir, $"{clienteCode}.png");

                // Génération synchrone (System.Drawing) déportée sur le thread pool
                await Task.Run(() => RenderCard(templatePath, codeBarre, x, y, outputPath));

                _logger.LogInformation("Image carte générée : {Path}", outputPath);

                var result = new CardImageResult
                {
                    // Chemin local toujours disponible pour voir l'image dans le navigateur
                    RelativeUrl = $"/cartes/{clienteCode}.png"
                };

                if (string.IsNullOrWhiteSpace(_media.PublicBaseUrl))
                    _logger.LogWarning("PublicBaseUrl non configurée — image générée mais non envoyée par WhatsApp.");
                else
                    result.PublicUrl = $"{_media.PublicBaseUrl.TrimEnd('/')}/cartes/{clienteCode}.png";

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur génération image carte - Code: {Code}", clienteCode);
                return null;
            }
        }

        private static void RenderCard(string templatePath, string barCode, int x, int y, string outputPath)
        {
            var barcodeSettings = new BarcodeSettings
            {
                Type = BarCodeType.EAN13,
                Data = barCode,
                BackColor = Color.Transparent,
                UseChecksum = CheckSumMode.ForceEnable,
                ShowTextOnBottom = true,
                TextAlignment = StringAlignment.Center,
                Unit = GraphicsUnit.Pixel,
                BarHeight = 295f,
                X = 6f,
                TextFont = new Font("Arial Sans Serif", 45),
                ImageHeight = 400f,
                ImageWidth = 800f,
                AutoResize = false
            };

            var generator = new BarCodeGenerator(barcodeSettings);

            using Image barCodeImage = generator.GenerateImage();
            using Image cardImage = Image.FromFile(templatePath);

            using (var graphics = Graphics.FromImage(cardImage))
                graphics.DrawImage(barCodeImage, new Rectangle(x, y, barCodeImage.Width, barCodeImage.Height));

            cardImage.Save(outputPath, ImageFormat.Png);
        }

        /// <summary>
        /// Template + position (X, Y) du code-barres selon le type de carte (repris de l'ancien projet).
        /// </summary>
        private static (string template, int x, int y) GetTemplate(int carteTypeId) => (CarteType)carteTypeId switch
        {
            CarteType.BRICOMAM3ALEM  => ("carte-bricomam3alem.png", 1450, 539),
            CarteType.BRICOMAARTISAN => ("carte-bricomaartisan.jpg", 1500, 1000),
            _                        => ("carte-amibricoma.png", 1450, 605),
        };
    }
}
