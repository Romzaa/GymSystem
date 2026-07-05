using GymSystem.BLL.ViewModels.AccountViewModels;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                             ILogger<AccountController> logger,
                                             SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct= default)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            { 
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }

           var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (signInResult.Succeeded)
            {
                _logger.LogInformation($" User {user.Email} logged in successfully.");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (signInResult.IsLockedOut)
            {
                _logger.LogInformation($" User {user.Email} account is locked out.");
                ModelState.AddModelError("InvalidLogin", "This Account Is Temporary Locked, Try Again Later");
            } else if (signInResult.IsNotAllowed)
            {
                ModelState.AddModelError("InvalidLogin", "Sign In Is Not Allowed For This Account");
            }
            else { ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password"); 
            }

            return View(model);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
           return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
