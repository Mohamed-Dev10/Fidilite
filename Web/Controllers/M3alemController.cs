using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Enum;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize(Policy = "carte.list")]
    public class M3alemController : Controller
    {
        private readonly IClienteBOService _clienteBOService;
        private readonly UserManager<ApplicationUser> _userManager;

        public M3alemController(IClienteBOService clienteBOService, UserManager<ApplicationUser> userManager)
        {
            _clienteBOService = clienteBOService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search, int? statutId, int? magasinId, int page = 1)
        {
            // Un responsable de magasin ne voit que les cartes de son magasin.
            // L'admin (SUPER_ADMIN) garde la vue globale et peut filtrer librement.
            if (!User.IsInRole("SUPER_ADMIN"))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                magasinId = currentUser?.RefMagasinId ?? -1;
            }

            var filter = new CarteListFilterModel
            {
                CarteTypeId = (int)CarteType.BRICOMAM3ALEM,
                Search = search,
                StatutId = statutId,
                MagasinId = magasinId,
                Page = 1,
                PageSize = 5000
            };

            var result = await _clienteBOService.GetList(filter);
            ViewData["Filter"] = filter;
            if (!result.Success)
                ViewData["Error"] = result.Message;
            return View(result.Data);
        }
    }
}
