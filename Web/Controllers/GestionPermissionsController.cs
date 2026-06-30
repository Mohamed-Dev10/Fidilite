using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize(Policy = "admin.roles")]
    public class GestionPermissionsController : Controller
    {
        private readonly IPermissionBOService _permissionBOService;
        private readonly IClienteBOService _clienteBOService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GestionPermissionsController(IPermissionBOService permissionBOService, IClienteBOService clienteBOService, UserManager<ApplicationUser> userManager)
        {
            _permissionBOService = permissionBOService;
            _clienteBOService = clienteBOService;
            _userManager = userManager;
        }

        // ── RÔLES ────────────────────────────────────────────────

        public async Task<IActionResult> Index()
        {
            var rolesResult = await _permissionBOService.GetAllRoles();
            var roles = rolesResult.Data ?? new List<ApplicationRole>();

            var permsResult = await _permissionBOService.GetAllPermissions();

            // Compteur pour le badge de l'onglet (sans charger la liste).
            ViewBag.UsersCount = await _permissionBOService.CountUsers(null, null);

            ViewBag.Permissions = permsResult.Data ?? new List<Permission>();
            return View(roles);
        }

        // Liste des utilisateurs paginée côté serveur (SQL Skip/Take) + rôles/magasins chargés
        // en lot (2 requêtes au lieu d'une par utilisateur) : reste rapide même à des dizaines
        // de milliers de comptes.
        [Authorize(Policy = "admin.users")]
        [HttpGet]
        public async Task<IActionResult> UsersData(string? search, string? role, int page = 1, int pageSize = 10)
        {
            var usersResult = await _permissionBOService.GetUsersPage(page, pageSize, search, role);
            var users = usersResult.Data ?? new List<ApplicationUser>();
            var total = await _permissionBOService.CountUsers(search, role);

            var userIds = users.Select(u => u.Id).ToList();
            var rolesMap = await _permissionBOService.GetRolesForUsers(userIds);
            var magasinsMap = await _clienteBOService.GetUserMagasinsByIds(userIds);

            var magasinsResult = await _clienteBOService.GetAllMagasins();
            var magasinNames = (magasinsResult.Data ?? new List<BRICOMA.ECOMMERCE.Data.Models.RefMagasin>())
                .ToDictionary(m => m.Id, m => m.Name);

            var items = users.Select(u => new
            {
                u.Id,
                u.Email,
                Role = rolesMap.TryGetValue(u.Id, out var r) ? r : "-",
                Magasin = magasinsMap.TryGetValue(u.Id, out var mid) && mid.HasValue && magasinNames.TryGetValue(mid.Value, out var mn) ? mn : null,
                Suspended = u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow
            });

            return Json(new
            {
                items,
                totalCount = total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            var result = await _permissionBOService.CreateRole(name);
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _permissionBOService.DeleteRole(id);
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // ── PERMISSIONS ──────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> AssignerPermissions(string id)
        {
            var roles = await _permissionBOService.GetAllRoles();
            var role = roles.Data?.FirstOrDefault(r => r.Id == id);
            if (role == null) return NotFound();

            var allPermissions = await _permissionBOService.GetAllPermissions();
            var assigned = await _permissionBOService.GetRolePermissions(id);

            ViewBag.AllPermissions = allPermissions.Data ?? new List<Permission>();
            ViewBag.AssignedIds = assigned.Data ?? new List<int>();
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> AssignerPermissions(string id, List<int> permissionIds)
        {
            var result = await _permissionBOService.SetRolePermissions(id, permissionIds ?? new List<int>());
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // ── UTILISATEURS ─────────────────────────────────────────

        [Authorize(Policy = "admin.users")]
        public async Task<IActionResult> Utilisateurs()
        {
            var usersResult = await _permissionBOService.GetAllUsers();
            var users = usersResult.Data ?? new List<ApplicationUser>();

            var userWithRoles = new List<(ApplicationUser User, string Role)>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add((user, roles.FirstOrDefault() ?? "-"));
            }

            ViewBag.UserWithRoles = userWithRoles;
            return View();
        }

        [Authorize(Policy = "admin.users")]
        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            await LoadDropdowns();
            return View(new CreateUserModel());
        }

        [Authorize(Policy = "admin.users")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            var result = await _permissionBOService.CreateUser(model);
            if (!result.Data)
            {
                ViewData["Error"] = result.Message;
                await LoadDropdowns();
                return View(model);
            }
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "admin.users")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var usersResult = await _permissionBOService.GetAllUsers();
            var user = usersResult.Data?.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var rolesResult = await _permissionBOService.GetAllRoles();
            var userRole = rolesResult.Data?.FirstOrDefault(r => r.Name == roles.FirstOrDefault());

            // Infos profil (nom / prénom / magasin) lues depuis la table Profil (source officielle).
            var profil = await _clienteBOService.GetUserProfil(user.Id);
            var model = new UpdateUserModel
            {
                Id = user.Id,
                Nom = profil?.Nom ?? string.Empty,
                Prenom = profil?.Prenom ?? string.Empty,
                Email = user.Email ?? string.Empty,
                RoleId = userRole?.Id ?? string.Empty,
                RefMagasinId = profil?.RefMagasinId
            };

            await LoadDropdowns();
            return View(model);
        }

        [Authorize(Policy = "admin.users")]
        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserModel model)
        {
            var result = await _permissionBOService.UpdateUser(model);
            if (!result.Data)
            {
                ViewData["Error"] = result.Message;
                await LoadDropdowns();
                return View(model);
            }
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "admin.users")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentId = _userManager.GetUserId(User);
            if (currentId == id)
            {
                TempData["Error"] = "Vous ne pouvez pas supprimer votre propre compte.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _permissionBOService.DeleteUser(id);
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "admin.users")]
        [HttpPost]
        public async Task<IActionResult> ToggleUserSuspension(string id)
        {
            var currentId = _userManager.GetUserId(User);
            if (currentId == id)
            {
                TempData["Error"] = "Vous ne pouvez pas suspendre votre propre compte.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _permissionBOService.ToggleUserSuspension(id);
            if (result.Data)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            var roles = await _permissionBOService.GetAllRoles();
            var magasins = await _clienteBOService.GetAllMagasins();
            ViewBag.Roles = new SelectList(roles.Data ?? new List<ApplicationRole>(), "Id", "Name");
            ViewBag.Magasins = new SelectList(magasins.Data ?? new List<BRICOMA.ECOMMERCE.Data.Models.RefMagasin>(), "Id", "Name");
        }
    }
}
