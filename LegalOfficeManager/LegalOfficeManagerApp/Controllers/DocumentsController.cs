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
                    Console.WriteLine("Inner Exception: " + inner);
                    throw; 
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
            ViewBag.BaseUrl = $"{Request.Scheme}://{Request.Host}";
            return View(documents);
        }



        public async Task<IActionResult> Download(Guid id)
        {
            var document = await _db.Documents.FindAsync(id);
            if (document == null)
                return NotFound();

            var relativePath = document.FilePath.TrimStart('/', '\\');
            var filePath = Path.Combine(_env.WebRootPath, relativePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(document.FileName);
            return File(fileBytes, contentType, document.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var document = await _db.Documents.FindAsync(id);
            if (document == null)
                return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, document.FilePath.TrimStart('/', '\\'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _db.Documents.Remove(document);
            await _db.SaveChangesAsync();

            return RedirectToAction("List", new { legalOfficeEntryId = document.LegalOfficeEntryId });
        }



        private string GetContentType(string fileName)
        {
            var types = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        {".pdf", "application/pdf"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".doc", "application/msword"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".xls", "application/vnd.ms-excel"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".txt", "text/plain"},
    };

            var ext = Path.GetExtension(fileName);
            return types.TryGetValue(ext, out var contentType) ? contentType : "application/octet-stream";
        }




    }

}
