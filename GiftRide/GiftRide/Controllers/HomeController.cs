using System.Diagnostics;
using GiftRide.Core.Contracts;
using GiftRide.Models;
using GiftRide.Models.Contact;
using Microsoft.AspNetCore.Mvc;

namespace GiftRide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactService _contactService;



        public HomeController(ILogger<HomeController> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
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

            bool isSaved = _contactService.SaveMessage(
                model.Name,
                model.Email,
                model.Phone,
                model.Subject,
                model.Message
            );

            if (isSaved)
            {
                TempData["SuccessMessage"] = $"Благодарим Ви, {model.Name}! Вашето съобщение е получено.";
            }
            else
            {
                TempData["ErrorMessage"] = "Възникна грешка при записването в базата данни.";
            }

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
