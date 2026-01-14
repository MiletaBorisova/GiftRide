using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data.Entities;
using GiftRide.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace GiftRide.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderController(IProductService productService, IOrderService orderService, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }


        // GET: OrderController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            //string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = context.Users.SingleOrDefault(u => u.Id == userId);

            List<OrderIndexVM> orders = _orderService.GetOrders()
                .Select(x => new OrderIndexVM
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
                    UserId = x.UserId,
                    User = x.User.UserName,
                    ProductId = x.ProductId,
                    Product = x.Product.ProductName,
                    Picture = x.Product.Picture,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Discount = x.Discount,
                    TotalPrice = x.TotalPrice,


                }).ToList();

            return View(orders);

        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrdersByUser(userId);

            var model = orders.Select(o =>
            {
                // Взимаме първия ваучер от списъка (защото UI-ът ти показва 1 ред за поръчка)
                var voucher = o.Vouchers.FirstOrDefault();

                return new OrderIndexVM
                {
                    Id = o.Id, // Предполагам, че имаш Id в Order entity
                    OrderDate = o.OrderDate.ToString("dd.MM.yyyy"),
                    Product = o.Product.ProductName,
                    Picture = o.Product?.Picture,
                    Quantity = o.Quantity,
                    Price = o.TotalPrice,

                    // ТУК Е МАГИЯТА ЗА БУТОНА:
                    // Ако има ваучер, взимаме неговото ID, ако не - 0
                    VoucherId = voucher?.Id ?? 0,
                    VoucherCode = voucher?.VoucherCode,

                    // Взимаме статуса от ваучера
                    Status = voucher?.Status ?? ReservationStatus.None,

                    // За да показваш датата, ако е резервиран
                    ReservationDate = voucher?.ReservationDate
                };
            }).ToList();

            return View(model);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create(int id)
        {
            Product product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            OrderCreateVM order = new OrderCreateVM()
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                QuantityInStock = product.Quantity,
                Picture = product.Picture,
                Discount = product.Discount,
                Price = product.Price
            };
            return View(order);
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreateVM bindingModel)
        {

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = this._productService.GetProductById(bindingModel.ProductId);

            if (currentUserId == null || product == null || product.Quantity < bindingModel.Quantity ||
                product.Quantity == 0)
            {
                return RedirectToAction("Denied", "Order");
            }
            if (ModelState.IsValid)
            {
                _orderService.Create(bindingModel.ProductId, currentUserId, bindingModel.Quantity);
            }
            return this.RedirectToAction("Index", "Product");
        }

        //GET: OrderController/Denied
        public ActionResult Denied()
        {
            return View();
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateFromCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (!cart.Items.Any())
                return RedirectToAction("Index", "Cart");
            decimal promoDiscount = cart.AppliedPromoDiscountPercent;

            foreach (var item in cart.Items)
            {

                _orderService.CreateFromCartItem(item, userId, promoDiscount);
            }


            await _cartService.ResetCartAsync(userId);

            return RedirectToAction("Success", "Order");
        }
        public IActionResult Success()
        {
            return View();
        }

    }
}
