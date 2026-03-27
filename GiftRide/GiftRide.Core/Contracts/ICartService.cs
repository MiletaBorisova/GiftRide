using GiftRide.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Contracts
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task AddItemAsync(string userId, int productId, int quantity = 1);
        Task RemoveItemAsync(string userId, int productId);
        Task UpdateQuantityAsync(string userId, int productId, int quantity);
        Task<decimal> GetTotalAsync(string userId);
       
        decimal CalculateTotalWithPromo(Cart cart);
        Task ResetCartAsync(string userId);

        Task ApplyPromoCodeAsync(string userId, int discountPercent);
    }
}
