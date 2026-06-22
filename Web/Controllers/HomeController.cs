using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BRICOMA.ECOMMERCE.Web.Models;
using BRICOMA.ECOMMERCE.Business.Interfaces;

namespace BRICOMA.ECOMMERCE.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IClienteBOService _clienteBOService;

    public HomeController(ILogger<HomeController> logger, IClienteBOService clienteBOService)
    {
        _logger = logger;
        _clienteBOService = clienteBOService;
    }

    [Authorize(Roles = "SUPER_ADMIN")]
    public async Task<IActionResult> Index()
    {
        var result = await _clienteBOService.GetDashboardStats();
        return View(result.Data);
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
