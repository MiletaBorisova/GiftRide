using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data;
using GiftRide.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(string name, string description, int validityId, int categoryId, string picture,
            int quantity, decimal price, decimal discount)
        {
            Product item = new Product
            {
                ProductName = name,
                Description = description,
                Validity = _context.Validities.Find(validityId),
                Category = _context.Categories.Find(categoryId),
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount

            };
            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }
        public List<Product> GetProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }
        public List<Product> GetProducts(string searchStringCategoryName, string searchStringValidityName)
        {
            List<Product> products = _context.Products.ToList();
            if (!String.IsNullOrEmpty(searchStringCategoryName) && !String.IsNullOrEmpty(searchStringValidityName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains
                (searchStringCategoryName.ToLower())
                && x.Validity.ValidityName.ToLower().Contains(searchStringValidityName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringCategoryName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains
                (searchStringCategoryName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringValidityName))
            {
                products = products.Where(x => x.Validity.ValidityName.ToLower().Contains
                (searchStringValidityName.ToLower())).ToList();
            }
            return products;
        }

       
        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int productId, string name, string description, int validityId, int categoryId, string picture,
            int quantity, decimal price, decimal discount)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            product.ProductName = name;
            product.Description = description;
            product.Validity = _context.Validities.Find(validityId);
            product.Category = _context.Categories.Find(categoryId);
            product.Picture = picture;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;
            _context.Update(product);
            return _context.SaveChanges() != 0;
        }

        public bool HasOrders(int productId)
        {
            return _context.Orders.Any(x => x.ProductId == productId);
        }

       
    }
}