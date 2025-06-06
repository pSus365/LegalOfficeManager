using LegalOfficeManagerApp.Data;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LegalOfficeManagerApp.Controllers
{
    public class LegalOfficeEntriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public LegalOfficeEntriesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)  // dependency injection of the database context!
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<LegalOfficeEntry> objLegalOfficeEntryList = _db.LegalOfficeEntries.ToList();
            return View(objLegalOfficeEntryList);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LegalOfficeEntry obj)
        {
            if(obj != null && obj.Name.Length < 3)
            {
                ModelState.AddModelError("Name", "Name must be at least 3 characters long.");
            }

            if (ModelState.IsValid) 
            {
                _db.LegalOfficeEntries.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == null)
            {
                return NotFound();
            }

            LegalOfficeEntry? legalOfficeEntry = _db.LegalOfficeEntries.Find(id);
            
            if (legalOfficeEntry == null)
            {
                return NotFound();
            }

            return View(legalOfficeEntry);
        }

        [HttpPost]
        public IActionResult Edit(LegalOfficeEntry obj)
        {
            if (obj != null && obj.Name.Length < 3)
            {
                ModelState.AddModelError("Name", "Name must be at least 3 characters long.");
            }

            if (ModelState.IsValid) 
            {
                _db.LegalOfficeEntries.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == null)
            {
                return NotFound();
            }

            LegalOfficeEntry? legalOfficeEntry = _db.LegalOfficeEntries.Find(id);

            if (legalOfficeEntry == null)
            {
                return NotFound();
            }

            return View(legalOfficeEntry);
        }

        [HttpPost]
        public IActionResult Delete(LegalOfficeEntry obj)
        {
            _db.LegalOfficeEntries.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult PriceListView() {
            return View();
        }

        public IActionResult ChoosePackage(string package)
        {
            if (string.IsNullOrEmpty(package))
            {
                return RedirectToAction("PriceListView");
            }

            return RedirectToAction("FakePayment", new { package = package });
        }

        [HttpGet]
        public IActionResult FakePayment(string package)
        {
            if (string.IsNullOrEmpty(package))
                return RedirectToAction("PriceListView");

            var model = new PaymentViewModel { Package = package };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FakePayment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.ActivePackage = model.Package;
            _db.Update(user);
            await _db.SaveChangesAsync();

            ViewData["ShowThankYouModal"] = true;
            return View(model);
        }


    }
}
