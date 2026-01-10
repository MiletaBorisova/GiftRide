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
    public class ValidityService : IValidityService
    {
        private readonly ApplicationDbContext _context;
        public ValidityService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Product> GetProductsByValidity(int validityId)
        {
            return _context.Products
               .Where(x => x.ValidityId == validityId)
               .ToList();
        }

        public List<Validity> GetValidities()
        {
            List<Validity> validities = _context.Validities.ToList();
            return validities;
        }

        public Validity GetValidityById(int validityId)
        {
            return _context.Validities.Find(validityId);
        }
    }
}
