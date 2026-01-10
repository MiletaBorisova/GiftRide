using GiftRide.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Contracts
{
    public interface IValidityService
    {
        List<Validity> GetValidities();
        Validity GetValidityById(int validityId);
        List<Product> GetProductsByValidity(int validityId);
    }
}
