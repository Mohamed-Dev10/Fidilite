using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BRICOMA.ECOMMERCE.Web.Models;
using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IClienteBOService _clienteBOService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IClienteBOService clienteBOService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _clienteBOService = clienteBOService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // Admin : vue globale (tous magasins).
        if (User.IsInRole("SUPER_ADMIN"))
        {
            var globalResult = await _clienteBOService.GetDashboardStats();
            return View(globalResult.Data);
        }

        // Utilisateur magasin : KPI filtrés sur son propre magasin (lu depuis Profil).
        var userId = _userManager.GetUserId(User);
        var magasinId = await _clienteBOService.GetUserMagasinId(userId);
        if (magasinId == null)
        {
            // Aucun magasin assigné : on affiche un message, pas de données globales.
            return View(new DashboardStatsModel { EstGlobal = false, SansMagasin = true });
        }

        var result = await _clienteBOService.GetDashboardStats(magasinId);
        return View(result.Data ?? new DashboardStatsModel { EstGlobal = false });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
