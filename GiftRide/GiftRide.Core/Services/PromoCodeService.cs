using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Services
{
    public class PromoCodeService : IPromoCodeService
    {     
        private readonly ApplicationDbContext _context;

        public PromoCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetDiscountPercentAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return 0;
            }

            var promo = await _context.PromoCodes
            .FirstOrDefaultAsync(p => p.Code.ToLower() == code.ToLower() && p.IsActive);

            if (promo == null)
            {
                return 0;
            }

            return promo.DiscountPercent;
        }
    }
    
}
