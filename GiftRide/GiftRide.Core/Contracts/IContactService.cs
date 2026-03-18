using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GiftRide.Core.Contracts
{
    public interface IContactService
    {
        bool SaveMessage(string name,string email, string phone, string subject, string message);
    }
}
