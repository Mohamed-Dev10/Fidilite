using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Data.Models;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize]
    public class CarteController : Controller
    {
        private readonly IClienteBOService _clienteBOService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarteController(IClienteBOService clienteBOService, UserManager<ApplicationUser> userManager)
        {
            _clienteBOService = clienteBOService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "carte.create")]
        public async Task<IActionResult> Creer(int? carteTypeId = null)
        {
            await LoadDropdowns();
            var model = new ClienteModel();
            if (carteTypeId.HasValue)
                model.RefCarteTypeId = carteTypeId;

            // Un responsable de magasin crée toujours pour son propre magasin (pré-sélectionné).
            var scopeMagasinId = await GetScopeMagasinId();
            if (scopeMagasinId.HasValue)
                model.RefMagasinId = scopeMagasinId.Value;

            return View(model);
        }

        // Étape 1 : valide + envoie l'OTP par WhatsApp
        [HttpPost]
        [Authorize(Policy = "carte.create")]
        public async Task<IActionResult> Creer(ClienteModel model)
        {
            // Sécurité : le responsable ne peut créer une carte que pour son magasin,
            // quel que soit le magasin envoyé par le formulaire.
            var scopeMagasinId = await GetScopeMagasinId();
            if (scopeMagasinId.HasValue)
                model.RefMagasinId = scopeMagasinId.Value;

            var result = await _clienteBOService.InitCreate(model);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                await LoadDropdowns();
                return View(model);
            }

            // Affiche le formulaire de saisie OTP
            ViewBag.Token = result.Data;
            ViewBag.Gsm = model.Gsm;
            ViewData["Success"] = result.Message;
            return View("VerifierOtp");
        }

        // Renvoie un nouveau code OTP (le client ne l'a pas reçu) et réaffiche la saisie OTP
        [HttpPost]
        [Authorize(Policy = "carte.create")]
        public async Task<IActionResult> RenvoyerOtp(string token, string gsm)
        {
            var result = await _clienteBOService.ResendOtp(token);

            ViewBag.Token = token;
            ViewBag.Gsm = gsm;
            if (result.Success)
                ViewData["Success"] = result.Message;
            else
                ViewData["Error"] = result.Message;

            return View("VerifierOtp");
        }

        // Étape 2 : vérifie l'OTP et crée la carte (FIDELITE + MARKET + WhatsApp)
        [HttpPost]
        [Authorize(Policy = "carte.confirm")]
        public async Task<IActionResult> VerifierOtp(string token, string otpCode, string gsm)
        {
            var result = await _clienteBOService.ConfirmCreate(token, otpCode);
            if (!result.Success)
            {
                ViewBag.Token = token;
                ViewBag.Gsm = gsm;
                ViewData["Error"] = result.Message;
                return View("VerifierOtp");
            }

            TempData["Success"] = result.Message;
            // Chemin local de l'image générée → affichée en aperçu sur la page de création
            if (!string.IsNullOrWhiteSpace(result.Data))
                TempData["CarteImageUrl"] = result.Data;
            return RedirectToAction(nameof(Creer));
        }

        // Liste générique des cartes d'un type donné (M3alem, Artisan, ou tout type paramétrable).
        // Utilisée par les KPI dynamiques du dashboard.
        [HttpGet]
        [Authorize(Policy = "carte.list")]
        public async Task<IActionResult> Liste(int typeId, string? search, int? statutId, int page = 1)
        {
            // Un responsable de magasin ne voit que les cartes de son magasin.
            var scopeMagasinId = await GetScopeMagasinId();
            int? magasinId = scopeMagasinId.HasValue ? scopeMagasinId : null;
            if (!User.IsInRole("SUPER_ADMIN") && !scopeMagasinId.HasValue)
                magasinId = -1; // responsable sans magasin → liste vide

            var filter = new CarteListFilterModel
            {
                CarteTypeId = typeId,
                Search = search,
                StatutId = statutId,
                MagasinId = magasinId,
                Page = 1,
                PageSize = 5000
            };

            var result = await _clienteBOService.GetList(filter);
            ViewData["Filter"] = filter;

            var type = (await _clienteBOService.GetAllRefCarteTypes()).Data?.FirstOrDefault(t => t.Id == typeId);
            ViewData["TypeId"] = typeId;
            ViewData["TypeName"] = type?.Name ?? "Cartes";

            if (!result.Success)
                ViewData["Error"] = result.Message;
            return View(result.Data);
        }

        // Détail d'une carte (infos + image)
        [HttpGet]
        [Authorize(Policy = "carte.list")]
        public async Task<IActionResult> Detail(long id)
        {
            var result = await _clienteBOService.GetById(id);
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            return View(result.Data);
        }

        // Édition d'une carte (GET)
        [HttpGet]
        [Authorize(Policy = "carte.edit")]
        public async Task<IActionResult> Modifier(long id)
        {
            var result = await _clienteBOService.GetById(id);
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }

            await LoadDropdowns();
            return View(ToModel(result.Data));
        }

        // Édition d'une carte (POST) → FIDELITE + MARKET
        [HttpPost]
        [Authorize(Policy = "carte.edit")]
        public async Task<IActionResult> Modifier(ClienteModel model)
        {
            var result = await _clienteBOService.UpdateCarte(model);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                await LoadDropdowns();
                return View(model);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Detail), new { id = model.Id });
        }

        private static ClienteModel ToModel(Cliente c) => new ClienteModel
        {
            Id = c.Id,
            Code = c.Code,
            CodeBarre = c.CodeBarre,
            Nom = c.Nom,
            Prenom = c.Prenom,
            Gsm = c.Gsm,
            Cin = c.Cin,
            Nia = c.Nia,
            Email = c.Email,
            Adresse = c.Adresse,
            Fonction = c.Fonction,
            RaisonSociale = c.RaisonSociale,
            DateNaissance = c.DateNaissance,
            RefGenreId = c.RefGenreId,
            RefVilleId = c.RefVilleId,
            RefMagasinId = c.RefMagasinId,
            RefMetierId = c.RefMetierId,
            RefCarteTypeId = c.RefCarteTypeId,
            RefClienteStatutId = c.RefClienteStatutId,
            IsActif = c.IsActif ?? true
        };

        private async Task LoadDropdowns()
        {
            var types    = await _clienteBOService.GetAllRefCarteTypes();
            var magasins = await _clienteBOService.GetAllMagasins();
            var genres   = await _clienteBOService.GetAllGenres();
            var metiers  = await _clienteBOService.GetAllMetiers();
            var villes   = await _clienteBOService.GetAllVilles();

            var magasinList = magasins.Data ?? new List<RefMagasin>();

            // Un responsable de magasin ne voit que son magasin dans la liste déroulante.
            var scopeMagasinId = await GetScopeMagasinId();
            if (scopeMagasinId.HasValue)
                magasinList = magasinList.Where(m => m.Id == scopeMagasinId.Value).ToList();

            // AMIBRICOMA (id 1) n'est plus proposé à la création dans le back-office
            ViewBag.CarteTypes = new SelectList(
                (types.Data ?? new List<RefCarteType>()).Where(t => t.Id != (int)BRICOMA.ECOMMERCE.Models.Enum.CarteType.AMIBRICOMA),
                "Id", "Name");
            ViewBag.Magasins   = new SelectList(magasinList,                                "Id", "Name");
            ViewBag.Genres     = new SelectList(genres.Data   ?? new List<RefGenre>(),     "Id", "Name");
            ViewBag.Metiers    = new SelectList(metiers.Data  ?? new List<RefMetier>(),    "Id", "Name");
            ViewBag.Villes     = new SelectList(villes.Data   ?? new List<RefVille>(),     "Id", "Name");
        }

        // null  => admin (accès global, tous les magasins)
        // valeur => responsable rattaché à ce magasin (scope imposé)
        private async Task<int?> GetScopeMagasinId()
        {
            if (User.IsInRole("SUPER_ADMIN"))
                return null;
            var userId = _userManager.GetUserId(User);
            return await _clienteBOService.GetUserMagasinId(userId);
        }
    }
}
