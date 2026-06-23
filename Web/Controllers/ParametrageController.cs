using BRICOMA.ECOMMERCE.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize(Policy = "parametrage.view")]
    public class ParametrageController : Controller
    {
        private readonly IClienteBOService _clienteBOService;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] ImagesAutorisees = { ".png", ".jpg", ".jpeg" };

        public ParametrageController(IClienteBOService clienteBOService, IWebHostEnvironment env)
        {
            _clienteBOService = clienteBOService;
            _env = env;
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
            return View(item);
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string messageReception,
                                              IFormFile image, int barcodeX = 50, int barcodeY = 50)
        {
            // 1) Nom du type
            var result = await _clienteBOService.UpdateRefCarteType(id, name);
            if (!result.Data)
            {
                ViewData["Error"] = result.Message;
                ViewBag.Parametrage = (await _clienteBOService.GetParametrage(id)).Data;
                var list = await _clienteBOService.GetAllRefCarteTypes();
                return View(list.Data?.FirstOrDefault(r => r.Id == id));
            }

            // 2) Image-modèle (optionnelle) : on conserve l'ancienne si aucune n'est envoyée.
            string imagePath = null;
            if (image != null && image.Length > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!ImagesAutorisees.Contains(ext))
                {
                    ViewData["Error"] = "Format d'image non supporté (PNG ou JPG uniquement).";
                    ViewBag.Parametrage = (await _clienteBOService.GetParametrage(id)).Data;
                    var list = await _clienteBOService.GetAllRefCarteTypes();
                    return View(list.Data?.FirstOrDefault(r => r.Id == id));
                }

                var dir = Path.Combine(_env.WebRootPath, "media", "types");
                Directory.CreateDirectory(dir);

                // On supprime les anciennes images de ce type (extension potentiellement différente).
                foreach (var old in Directory.GetFiles(dir, $"{id}.*"))
                {
                    try { System.IO.File.Delete(old); } catch { /* fichier verrouillé : ignoré */ }
                }

                var fileName = $"{id}{ext}";
                var fullPath = Path.Combine(dir, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await image.CopyToAsync(stream);

                imagePath = $"media/types/{fileName}";
            }

            // 3) Message + position du code-barres
            var saved = await _clienteBOService.SaveParametrage(id, messageReception, imagePath, barcodeX, barcodeY);
            if (!saved.Data)
                ViewData["Error"] = saved.Message;
            else
                TempData["Success"] = "Type de carte et paramétrage enregistrés.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clienteBOService.DeleteRefCarteType(id);
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
