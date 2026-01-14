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

        // GET: Otvaria se kalendara
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            Voucher voucher = null;
            if (id > 0)
            {
                voucher = _reservationService.GetVoucherForReservation(id, user.Id);
            }

            
            if (voucher != null && voucher.ExpiryDate >= DateTime.Now)
            {
                
                ViewBag.MinDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                ViewBag.MaxDate = voucher.ExpiryDate.ToString("yyyy-MM-dd");

                return View(voucher);
            }

            
            TempData["Error"] = "Ваучерът е невалиден или срокът му е изтекъл.";
            return RedirectToAction("MyOrders", "Order");
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> ConfirmReservation(int voucherId, DateTime reservationDate)
        {
          

            if (reservationDate <= DateTime.Now)
            {
                TempData["Error"] = "Моля, изберете валидна бъдеща дата.";
                return RedirectToAction("Create", "Reservation");
            }

            var user = await _userManager.GetUserAsync(User);

            bool success = _reservationService.MakeReservation(voucherId, reservationDate, user.Id);

            if (success)
            {
                TempData["Success"] = "Успешна резервация! Очаквайте потвърждение.";
                return RedirectToAction("MyOrders", "Order");
            }
            else
            {
                TempData["Error"] = "Грешка при резервацията. Проверете датата.";
                return RedirectToAction("Create", "Reservation");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult UpdateStatus(int voucherId, int statusId)
        {
            // Preobrazuva se chisloto kum enum-a za ReservationStatus
            ReservationStatus newStatus = (ReservationStatus)statusId;

            bool result = _reservationService.ChangeStatus(voucherId, newStatus);

            if (result)
            {
                TempData["Success"] = "Статусът е променен успешно!";
            }
            else
            {
                TempData["Error"] = "Възникна грешка при промяната.";
            }

            
            return RedirectToAction("Index", "Order");
        }
    }
}

