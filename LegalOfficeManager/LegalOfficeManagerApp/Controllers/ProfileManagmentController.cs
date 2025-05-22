using LegalOfficeManagerApp.Data;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegalOfficeManagerApp.Controllers
{
    public class ProfileManagmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileManagmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
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
    }
}
