using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data;
using GiftRide.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public bool Create(int productId, string userId, int quantity)
        {
            var product = this._context.Products
                .Include(p => p.Validity)
                .SingleOrDefault(x => x.Id == productId);

            if (product == null) return false;


            decimal priceWithDiscount = product.Price;
            if (product.Discount > 0)
            {
                priceWithDiscount = product.Price * (1 - (product.Discount / 100m));
            }

            Order item = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount,
                TotalPrice = priceWithDiscount * quantity
            };

            product.Quantity -= quantity;

            this._context.Products.Update(product);
            this._context.Orders.Add(item);

            //suzdavat se vaucherite
            int months = 3; 
            if (product.Validity != null)
            {
                var digits = new String(product.Validity.ValidityName.Where(Char.IsDigit).ToArray());
                if (!string.IsNullOrEmpty(digits)) months = int.Parse(digits);
            }

            for (int i = 0; i < quantity; i++)
            {
                var voucher = new Voucher
                {
                    Product = product,
                    Order = item, // Svurzva se s poruchkata
                    PurchaseDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddMonths(months),
                    Status = ReservationStatus.None
                };
                this._context.Vouchers.Add(voucher);
            }

            return _context.SaveChanges() != 0;
        }


        public List<Order> GetOrders()
        {
            return _context.Orders
                .Include(x => x.Product)  
                .Include(x => x.User)     
                .Include(x => x.Vouchers) 
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }
        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders
                     .Include(x => x.Vouchers) //Zarejda vaucherite kum poruchkata
                     .ThenInclude(v => v.Product) 
                     .Where(x => x.UserId == userId)
                     .OrderByDescending(x => x.OrderDate)
                     .ToList();
        }

        public bool CreateFromCartItem(CartItem cartItem, string userId, decimal promoDiscountPercent)
        {
            var product = _context.Products
                .Include(p => p.Validity)
                .FirstOrDefault(p => p.Id == cartItem.ProductId);
            if (product == null) return false;

            //discount zadaden ot create
            decimal productDiscount = product.Discount;


            decimal productPriceAfterDiscount = product.Price * (1 - (productDiscount / 100m));


            decimal finalUnitPrice = productPriceAfterDiscount * (1 - (promoDiscountPercent / 100m));


            decimal totalDiscountToRecord = productDiscount + promoDiscountPercent;

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = product.Id,
                UserId = userId,
                Quantity = cartItem.Quantity,
                Price = product.Price,

                //discount + promocode %
                Discount = totalDiscountToRecord,


                TotalPrice = finalUnitPrice * cartItem.Quantity
            };

            product.Quantity -= cartItem.Quantity;

            _context.Orders.Add(order);
            _context.Products.Update(product);

            //suzdavat se vaucherite
            int months = 3;
            if (product.Validity != null)
            {
                var digits = new String(product.Validity.ValidityName.Where(Char.IsDigit).ToArray());
                if (!string.IsNullOrEmpty(digits)) months = int.Parse(digits);
            }

            for (int i = 0; i < cartItem.Quantity; i++)
            {
                var voucher = new Voucher
                {
                    Product = product,
                    Order = order,
                    PurchaseDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddMonths(months),
                    Status = ReservationStatus.None
                };
                this._context.Vouchers.Add(voucher);
            }

            return _context.SaveChanges() > 0;
        }

    }
}
