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
            
            var ordersList = _orderService.GetOrders();

            List<OrderIndexVM> orders = ordersList
                .Select(x => 
                {
                    var voucher = x.Vouchers.FirstOrDefault();
                    return new OrderIndexVM
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
                        VoucherId = (voucher != null) ? voucher.Id : 0,
                        Status = (voucher != null) ? voucher.Status : ReservationStatus.None,
                        ReservationDate = (voucher != null) ? voucher.ReservationDate : null

                    };
                }).ToList();

            return View(orders);

        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrdersByUser(userId);

            var model = orders.Select(o =>
            {
                // Взима се първият ваучер от списъка
                var voucher = o.Vouchers.FirstOrDefault();

                return new OrderIndexVM
                {
                    Id = o.Id, 
                    OrderDate = o.OrderDate.ToString("dd.MM.yyyy"),
                    Product = o.Product.ProductName,
                    Picture = o.Product?.Picture,
                    Discount = o.Discount,
                    Quantity = o.Quantity,
                    Price = o.TotalPrice,

                    
                    // Ако има ваучер, взимаме неговото ID, ако не - 0
                    VoucherId = voucher?.Id ?? 0,
                    VoucherCode = voucher?.VoucherCode,

                    // Взимаме статуса от ваучера
                    Status = voucher?.Status ?? ReservationStatus.None,

                    // За да показва датата, ако е резервиран
                    ReservationDate = voucher?.ReservationDate
                };
            }).ToList();

            return View(model);
        }

      

        // GET: OrderController/Create
        public ActionResult Create(int id)
        {
            Product product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Quantity < 1)
            {
                return RedirectToAction("Denied", "Order"); 
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


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFromCart(GiftRide.Models.Cart.CartVM model)
        {
            
            var userId = _userManager.GetUserId(User);
            if (model.Items == null || !model.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            
            foreach (var itemVM in model.Items)
            {
                
                var cartItem = new CartItem
                {
                    ProductId = itemVM.ProductId,
                    Quantity = itemVM.Quantity,
                    Price = itemVM.Price
                };

                
                _orderService.CreateFromCartItem(cartItem, userId, model.AppliedPromoDiscountPercent);
            }

            await _cartService.ResetCartAsync(userId);

            TempData["Success"] = "Поръчката е създадена успешно!";       
            return RedirectToAction("MyOrders", "Order");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> CreateFromCart()
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var cart = await _cartService.GetCartByUserIdAsync(userId);


        //    if (!cart.Items.Any())
        //        return RedirectToAction("Index", "Cart");

        //    foreach (var item in cart.Items)
        //    {
        //        var productInDb = _productService.GetProductById(item.ProductId);
        //        if (productInDb == null || productInDb.Quantity < item.Quantity)
        //        {
        //            TempData["Error"] = $"Продуктът '{productInDb?.ProductName}' вече няма достатъчна наличност! Налични: {productInDb?.Quantity}";
        //            return RedirectToAction("Index", "Cart");
        //        }
        //    }

        //    decimal promoDiscount = cart.AppliedPromoDiscountPercent;

        //    foreach (var item in cart.Items)
        //    {

        //        _orderService.CreateFromCartItem(item, userId, promoDiscount);
        //    }


        //    await _cartService.ResetCartAsync(userId);

        //    return RedirectToAction("Success", "Order");
        //}
        public IActionResult Success()
        {
            return View();
        }
        //GET: OrderController/Denied
        public ActionResult Denied()
        {
            return View();
        }

    }
}
