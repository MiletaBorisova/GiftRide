using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Contracts
{
    public interface IPromoCodeService
    {
        Task<int> GetDiscountPercentAsync(string code);
    }
}
