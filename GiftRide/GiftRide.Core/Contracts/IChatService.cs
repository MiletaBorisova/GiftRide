using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Core.Contracts
{
    public interface IChatService
    {
        Task<string> GetAnswerAsync(string userMessage);
    }
}
