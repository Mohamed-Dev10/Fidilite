using BRICOMA.ECOMMERCE.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize(Policy = "parametrage.view")]
    public class ParametrageController : Controller
    {
        private readonly IClienteBOService _clienteBOService;

        public ParametrageController(IClienteBOService clienteBOService)
        {
            _clienteBOService = clienteBOService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _clienteBOService.GetAllRefCarteTypes();
            return View(result.Data);
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
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _clienteBOService.GetAllRefCarteTypes();
            var item = list.Data?.FirstOrDefault(r => r.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name)
        {
            var result = await _clienteBOService.UpdateRefCarteType(id, name);
            if (!result.Data)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "parametrage.edit")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteBOService.DeleteRefCarteType(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
