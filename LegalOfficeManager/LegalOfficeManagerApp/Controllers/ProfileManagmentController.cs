using LegalOfficeManagerApp.Data;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LegalOfficeManagerApp.Controllers
{
    public class ProfileManagmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileManagmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Home", "Home");

            var userData = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return View(userData); // przekaż model do widoku
        }
        [HttpPost]
        public async Task<IActionResult> SaveChanges(ApplicationUser model, string action)
        {

            if (action == "logout")
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "LogInPage");
            }
            if (!ModelState.IsValid)
                return View("Index", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["ProfileUpdated"] = "Your changes have been updated!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            TempData["ProfileUpdated"] = "Twoje zmiany zostały zapisane!";
            return RedirectToAction("Index");

           //return View("Index", model);
        }
    }
}
