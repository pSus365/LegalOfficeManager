using LegalOfficeManagerApp.Mappers;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LegalOfficeManagerApp.Controllers
{
    public class LogInPageController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserMapper _userMapper;


        public LogInPageController(SignInManager<ApplicationUser> signInManager,
                                   UserManager<ApplicationUser> userManager,
                                   RoleManager<IdentityRole> roleManager,
                                   UserMapper userMapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userMapper = userMapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Niepoprawny Email lub hasło!!");
                return View(model);
            }

            // Spróbuj zalogować
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (result.RequiresTwoFactor)
            {
                // Zapisz tymczasowo Email do TempData do użycia przy 2FA
                TempData["EmailFor2FA"] = model.Email;
                return RedirectToAction("LoginWith2fa", "TwoFactorAuth");
            }
            else
            {
                ModelState.AddModelError("", "Niepoprawny Email lub hasło!!");
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel();

            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            model.Roles = roles;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View("Register", model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Użytkownik z tym adresem e-mail już istnieje.");
                model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View("Register", model);
            }

            var user = _userMapper.ToEntity(model); // mapperly z registerviewmodel do applicationuser;


            user.UserName = model.Email;
            user.ActivePackage = "Casual";  // hardkodujemy basic package - pozniej mozna kupic lepszy


            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);

                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                }

                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View("Register", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }



    }
}
