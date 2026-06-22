using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                // Le tableau de bord est réservé aux administrateurs.
                // Les autres rôles sont dirigés vers la liste des cartes.
                var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
                if (user != null && await _signInManager.UserManager.IsInRoleAsync(user, "SUPER_ADMIN"))
                    return RedirectToAction("Index", "Home");

                return RedirectToAction("Index", "M3alem");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Ce compte est suspendu. Contactez l'administrateur.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            // Affiché lorsqu'un utilisateur connecté tente d'ouvrir une page
            // pour laquelle il n'a pas la permission requise.
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login");

            ViewData["Warning"] = "Vous n'avez pas le droit d'accéder à cette page.";
            return View();
        }
    }
}
