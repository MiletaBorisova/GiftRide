using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data;
using GiftRide.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftRide.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _cartService = cartService;
            _userManager = userManager;
            _context = context;
        }

        //pomoshtem metod za namirane na potrebitel
        private string GetUserId() => _userManager.GetUserId(User);

        // GET: CartController
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            ViewBag.Total = _cartService.CalculateTotalWithPromo(cart);
            return View(cart);
        }

        public async Task<IActionResult> Add(int productId)
        {
            var userId = GetUserId();
            await _cartService.AddItemAsync(userId, productId);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            await _cartService.RemoveItemAsync(userId, productId);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                await _cartService.UpdateQuantityAsync(userId, productId, item.Quantity + 1);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                await _cartService.UpdateQuantityAsync(userId, productId, item.Quantity - 1);
            }
            if (item.Quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApplyPromoCode(string code)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (string.IsNullOrWhiteSpace(code))
            {
                cart.AppliedPromoDiscountPercent = 0m;
                await _context.SaveChangesAsync();
                TempData["PromoError"] = "Невалиден промо код.";
                return RedirectToAction("Index");
            }

            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == code && p.IsActive);

            if (promo == null)
            {
                cart.AppliedPromoDiscountPercent = 0m;
                await _context.SaveChangesAsync();
                TempData["PromoError"] = "Невалиден или изтекъл промо код.";
                return RedirectToAction("Index");
            }


            cart.AppliedPromoDiscountPercent = promo.DiscountPercent;
            await _context.SaveChangesAsync();

            TempData["PromoMessage"] = $"Промо кодът е приложен (-{promo.DiscountPercent}%)";

            return RedirectToAction("Index");
        }
      

    }
}
