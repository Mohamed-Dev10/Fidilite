using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Models.Enum;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize(Policy = "carte.list")]
    public class M3alemController : Controller
    {
        private readonly IClienteBOService _clienteBOService;

        public M3alemController(IClienteBOService clienteBOService)
        {
            _clienteBOService = clienteBOService;
        }

        public async Task<IActionResult> Index(string? search, int? statutId, int? magasinId, int page = 1)
        {
            var filter = new CarteListFilterModel
            {
                CarteTypeId = (int)CarteType.BRICOMAM3ALEM,
                Search = search,
                StatutId = statutId,
                MagasinId = magasinId,
                Page = page,
                PageSize = 20
            };

            var result = await _clienteBOService.GetList(filter);
            ViewData["Filter"] = filter;
            if (!result.Success)
                ViewData["Error"] = result.Message;
            return View(result.Data);
        }
    }
}
