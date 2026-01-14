using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiftRide.Controllers
{
    
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(IReservationService reservationService, UserManager<ApplicationUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }

        // GET: Отваря календара
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            // 1. Проверка за валидно ID (ако е 0, значи OrderController не е подал ваучер)
            if (id <= 0)
            {
                TempData["Error"] = "Невалиден ваучер.";
                return RedirectToAction("MyOrders", "Order");
            }

            var user = await _userManager.GetUserAsync(User);
            var voucher = _reservationService.GetVoucherForReservation(id, user.Id);

            if (voucher == null)
            {
                TempData["Error"] = "Ваучерът не е намерен или не е ваш.";
                return RedirectToAction("MyOrders", "Order");
            }

            if (voucher.ExpiryDate < DateTime.Now)
            {
                TempData["Error"] = "Ваучерът е изтекъл!";
                return RedirectToAction("MyOrders", "Order");
            }

            // Настройки за календара (да не може да избира минали дати)
            ViewBag.MinDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            ViewBag.MaxDate = voucher.ExpiryDate.ToString("yyyy-MM-dd");

            return View(voucher);
        }

        // POST: Записва резервацията
        [HttpPost]
        public async Task<IActionResult> ConfirmReservation(int voucherId, DateTime reservationDate)
        {
            var user = await _userManager.GetUserAsync(User);

            if (reservationDate <= DateTime.Now)
            {
                TempData["Error"] = "Моля, изберете валидна бъдеща дата.";
                return RedirectToAction("Create", new { id = voucherId });
            }

            bool success = _reservationService.MakeReservation(voucherId, reservationDate, user.Id);

            if (success)
            {
                TempData["Success"] = "Успешна резервация! Очаквайте потвърждение.";
                return RedirectToAction("MyOrders", "Order");
            }
            else
            {
                TempData["Error"] = "Грешка при резервацията. Проверете датата.";
                return RedirectToAction("Create", new { id = voucherId });
            }
        }
    }
}

