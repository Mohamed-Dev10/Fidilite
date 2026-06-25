using BRICOMA.ECOMMERCE.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize(Policy = "parametrage.view")]
    public class ParametrageController : Controller
    {
        private readonly IClienteBOService _clienteBOService;
        private readonly IWebHostEnvironment _env;
        private readonly IMemoryCache _cache;

        private static readonly string[] ImagesAutorisees = { ".png", ".jpg", ".jpeg" };

        public const int CarteLargeur = 2305;
        public const int CarteHauteur = 1427;

        public ParametrageController(IClienteBOService clienteBOService, IWebHostEnvironment env, IMemoryCache cache)
        {
            _clienteBOService = clienteBOService;
            _env = env;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _clienteBOService.GetAllRefCarteTypes();
            // AMIBRICOMA (id 1) n'est pas géré dans le back-office
            var data = result.Data?
                .Where(t => t.Id != (int)BRICOMA.ECOMMERCE.Models.Enum.CarteType.AMIBRICOMA)
                .ToList();
            return View(data);
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var result = await _clienteBOService.CreateRefCarteType(name);
            if (!result.Data)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            _cache.Remove("menu_carte_types");
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _clienteBOService.GetAllRefCarteTypes();
            var item = list.Data?.FirstOrDefault(r => r.Id == id);
            if (item == null) return NotFound();

            // Paramétrage existant (message, image, position du code-barres) pour pré-remplir la page.
            ViewBag.Parametrage = (await _clienteBOService.GetParametrage(id)).Data;
            ViewBag.CardWidth = CarteLargeur;
            ViewBag.CardHeight = CarteHauteur;
            return View(item);
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string messageReception,
                                              IFormFile image, int barcodeX = 50, int barcodeY = 50,
                                              int barcodeScale = 100, bool removeImage = false)
        {
            // Ré-affiche la page Paramétrer avec un message d'erreur (en conservant le contexte).
            async Task<IActionResult> ViewWithError(string msg)
            {
                ViewData["Error"] = msg;
                ViewBag.Parametrage = (await _clienteBOService.GetParametrage(id)).Data;
                ViewBag.CardWidth = CarteLargeur;
                ViewBag.CardHeight = CarteHauteur;
                var l = await _clienteBOService.GetAllRefCarteTypes();
                return View(l.Data?.FirstOrDefault(r => r.Id == id));
            }

            // 1) Nom du type
            var result = await _clienteBOService.UpdateRefCarteType(id, name);
            if (!result.Data)
                return await ViewWithError(result.Message);

            var dirTypes = Path.Combine(_env.WebRootPath, "media", "types");

            // 2) Image-modèle (optionnelle) : on conserve l'ancienne si aucune n'est envoyée.
            string imagePath = null;
            if (image != null && image.Length > 0)
            {
                // Une nouvelle image annule une éventuelle demande de suppression.
                removeImage = false;
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!ImagesAutorisees.Contains(ext))
                    return await ViewWithError("Format d'image non supporté (PNG ou JPG uniquement).");

                // L'image DOIT avoir exactement les dimensions de la carte, sinon le code-barres
                // (positionné en %) ne tomberait pas au bon endroit sur la carte générée.
                int imgW, imgH;
                using (var probeStream = image.OpenReadStream())
                using (var probe = System.Drawing.Image.FromStream(probeStream, false, false))
                {
                    imgW = probe.Width;
                    imgH = probe.Height;
                }
                if (imgW != CarteLargeur || imgH != CarteHauteur)
                    return await ViewWithError(
                        $"L'image doit faire exactement {CarteLargeur} × {CarteHauteur} px (dimensions de la carte). " +
                        $"Image fournie : {imgW} × {imgH} px.");

                Directory.CreateDirectory(dirTypes);

                // On supprime les anciennes images de ce type (extension potentiellement différente).
                foreach (var old in Directory.GetFiles(dirTypes, $"{id}.*"))
                {
                    try { System.IO.File.Delete(old); } catch { /* fichier verrouillé : ignoré */ }
                }

                var fileName = $"{id}{ext}";
                var fullPath = Path.Combine(dirTypes, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await image.CopyToAsync(stream);

                imagePath = $"media/types/{fileName}";
            }
            else if (removeImage && Directory.Exists(dirTypes))
            {
                // Suppression demandée sans nouvelle image : on retire le fichier physique.
                foreach (var old in Directory.GetFiles(dirTypes, $"{id}.*"))
                {
                    try { System.IO.File.Delete(old); } catch { /* fichier verrouillé : ignoré */ }
                }
            }

            // 3) Message + position + taille du code-barres
            var saved = await _clienteBOService.SaveParametrage(id, messageReception, imagePath, barcodeX, barcodeY, barcodeScale, removeImage);
            if (!saved.Data)
                ViewData["Error"] = saved.Message;
            else
                TempData["Success"] = "Type de carte et paramétrage enregistrés.";

            _cache.Remove("menu_carte_types");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clienteBOService.DeleteRefCarteType(id);
            _cache.Remove("menu_carte_types");
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
