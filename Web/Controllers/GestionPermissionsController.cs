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
            var result = await _permissionBOService.GetAllRoles();
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name, string? description)
        {
            await _permissionBOService.CreateRole(name, description);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            await _permissionBOService.DeleteRole(id);
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
            await _permissionBOService.SetRolePermissions(id, permissionIds ?? new List<int>());
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
            return RedirectToAction(nameof(Utilisateurs));
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

            var model = new UpdateUserModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                RoleId = userRole?.Id ?? string.Empty
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
            return RedirectToAction(nameof(Utilisateurs));
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
