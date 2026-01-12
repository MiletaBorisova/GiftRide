using System.Diagnostics;
using GiftRide.Models;
using GiftRide.Models.Contact;
using Microsoft.AspNetCore.Mvc;

namespace GiftRide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET
        [HttpGet]
        public IActionResult Contacts()
        {
            return View(new ContactFormVM());
        }

        // POST
        [HttpPost]
        public IActionResult Contacts(ContactFormVM model)
        {
            
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = $"Формата не е попълнена!";
                return View(model);
            }

            
            TempData["SuccessMessage"] = $"Благодарим ви, {model.Name}! Вашето съобщение е получено.";            
            
            return RedirectToAction("Contacts");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
