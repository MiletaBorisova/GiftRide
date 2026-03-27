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
        private readonly IPromoCodeService _promoCodeService;


        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, IPromoCodeService promoCodeService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _promoCodeService = promoCodeService;
        }

        //pomoshtem metod za namirane na potrebitel
        private string GetUserId() => _userManager.GetUserId(User);

        // GET: CartController

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            
            var finalTotal = _cartService.CalculateTotalWithPromo(cart);

            
            var cartVM = new GiftRide.Models.Cart.CartVM
            {
                AppliedPromoDiscountPercent = cart.AppliedPromoDiscountPercent,
                FinalTotal = finalTotal,
                Items = cart.Items.Select(i => new GiftRide.Models.Cart.CartItemVM
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product!.ProductName,
                    Picture = i.Product.Picture,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            
            return View(cartVM);
        }
        //public async Task<IActionResult> Index()
        //{
        //    var userId = GetUserId();
        //    var cart = await _cartService.GetCartByUserIdAsync(userId);
        //    ViewBag.Total = _cartService.CalculateTotalWithPromo(cart);
        //    return View(cart);
        //}

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
            var userId = GetUserId();           
            int discount = await _promoCodeService.GetDiscountPercentAsync(code);

            if (discount == 0)
            {
                
                await _cartService.ApplyPromoCodeAsync(userId, 0);
                TempData["PromoError"] = "Невалиден или изтекъл промо код.";
            }
            else
            {
                
                await _cartService.ApplyPromoCodeAsync(userId, discount);
                TempData["PromoMessage"] = $"Промо кодът е приложен (-{discount}%)";
            }

            return RedirectToAction("Index");
        }


    }
}
