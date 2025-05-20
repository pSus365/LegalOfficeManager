using LegalOfficeManagerApp.Data;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalOfficeManagerApp.Controllers
{
    public class LegalOfficeEntriesController : Controller
    {
        private readonly ApplicationDbContext _db; 
        public LegalOfficeEntriesController(ApplicationDbContext db)  // dependency injection of the database context!
        {
            _db = db;
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
            //server side data validation
            if(obj != null && obj.Name.Length < 3)
            {
                ModelState.AddModelError("Name", "Name must be at least 3 characters long.");
            }

            if (ModelState.IsValid) // saving data only when the model state is valid (everything went good)
            {
                _db.LegalOfficeEntries.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(obj);
        }
    }
}
