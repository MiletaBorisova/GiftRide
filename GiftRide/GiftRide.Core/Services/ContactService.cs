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
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool SaveMessage(string name, string email, string phone, string subject, string message)
        {
            var newMessage = new Contact
            {
                Name = name,
                Email = email,
                Phone = phone,
                Subject = subject,
                Message = message
            };
            _context.Contacts.Add(newMessage);
            return _context.SaveChanges() > 0;
        }
    }
}
