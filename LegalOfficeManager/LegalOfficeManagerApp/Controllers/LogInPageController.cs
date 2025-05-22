using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalOfficeManagerApp.Controllers
{
    public class LogInPageController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Email == "admin@admin.com" && model.PasswordHash == "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło");
                return View(model);
            }
        }
    }
}
