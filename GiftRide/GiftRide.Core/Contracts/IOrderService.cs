using GiftRide.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Contracts
{
    public interface IOrderService
    {
        bool Create(int productId, string userId, int quantity);
        List<Order> GetOrders();
        List<Order> GetOrdersByUser(string userId);
        public bool CreateFromCartItem(CartItem cartItem, string userId, decimal promoDiscountPercent);
    }
}
