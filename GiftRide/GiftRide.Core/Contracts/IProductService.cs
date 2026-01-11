using GiftRide.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Contracts
{
    public interface IProductService
    {
        bool Create(string name, string description, int validityId, int categoryId, string picture,
            int quantity, decimal price, decimal discount);

        bool Update(int productId, string name, string description, int validityId, int categoryId, string picture,
            int quantity, decimal price, decimal discount);

        List<Product> GetProducts();
        Product GetProductById(int productId);
        bool RemoveById(int productId);
        List<Product> GetProducts(string searchStringCategoryName, string searchStringValidityName);
    }
}
