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
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetAnswerAsync(string userMessage)
        {
            if(string.IsNullOrWhiteSpace(userMessage))
                return "Моля, напишете въпрос.";

            userMessage = userMessage.ToLower();

          
            if (userMessage.Contains("здравей") || userMessage.Contains("здрасти") || userMessage.Contains("hello"))
                return "Здравейте! Аз съм виртуалният асистент на GiftRide. Как мога да Ви помогна?";

          
            var allFaqs = await _context.FaqEntries.ToListAsync();

          
            foreach (var faq in allFaqs)
            {
                // Разбиваме ключовите думи: "валидност, срок" -> ["валидност", " срок"]
                var keywords = faq.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(k => k.Trim().ToLower());

                foreach (var word in keywords)
                {
                    if (userMessage.Contains(word))
                    {
                        return faq.Answer;
                    }
                }
            }

           
            return "Съжалявам, не разбрах въпроса. Моля, опитайте с други думи или се свържете с нас на contact@giftride.com.";
        }
    }
    
}
