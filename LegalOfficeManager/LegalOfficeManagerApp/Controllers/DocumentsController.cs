using LegalOfficeManagerApp.Data;
using Microsoft.AspNetCore.Mvc;
using LegalOfficeManagerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LegalOfficeManagerApp.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;


        public DocumentsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Upload(int legalOfficeEntryId)
        {
            ViewBag.LegalOfficeEntryId = legalOfficeEntryId;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(int legalOfficeEntryId, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = new Document
                {
                    Id = Guid.NewGuid(),
                    LegalOfficeEntryId = legalOfficeEntryId,
                    FileName = file.FileName,
                    FilePath = "/uploads/" + file.FileName,
                    UploadDate = DateTime.Now
                };

                try
                {
                    _db.Documents.Add(document);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    var inner = ex.InnerException?.Message;
                    // Wyświetl to w logu, konsoli, lub ViewBag / TempData
                    Console.WriteLine("Inner Exception: " + inner);
                    throw; // lub zwróć widok z błędem
                }


                return RedirectToAction("Index", "LegalOfficeEntries");
            }

            ViewBag.LegalOfficeEntryId = legalOfficeEntryId;
            return View();
        }

        public async Task<IActionResult> List(int legalOfficeEntryId)
        {
            var documents = await _db.Documents
                .Where(d => d.LegalOfficeEntryId == legalOfficeEntryId)
                .ToListAsync();

            ViewBag.LegalOfficeEntryId = legalOfficeEntryId;
            return View(documents);
        }

    }

}
