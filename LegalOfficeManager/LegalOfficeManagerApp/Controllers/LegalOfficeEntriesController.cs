using Microsoft.AspNetCore.Mvc;

namespace LegalOfficeManagerApp.Controllers
{
    public class LegalOfficeEntriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
