using LegalOfficeManagerApp.Data;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalOfficeManagerApp.Controllers
{
    public class LegalOfficeEntriesController : Controller
    {
        private readonly ApplicationDbContext _db; 
        public LegalOfficeEntriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<LegalOfficeEntry> objLegalOfficeEntryList = _db.LegalOfficeEntries.ToList();
            return View(objLegalOfficeEntryList);

        }
    }
}
