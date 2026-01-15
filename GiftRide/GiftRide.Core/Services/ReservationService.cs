using GiftRide.Core.Contracts;
using GiftRide.Infrastructure.Data;
using GiftRide.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GiftRide.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public Voucher GetVoucherForReservation(int voucherId, string userId)
        {
            return _context.Vouchers
                .Include(v => v.Product) 
                .Include(v => v.Order)   
                .FirstOrDefault(v => v.Id == voucherId && v.Order.UserId == userId);
        }

       
        public bool MakeReservation(int voucherId, DateTime date, string userId)
        {
           
            var voucher = _context.Vouchers
                 .Include(v => v.Order)
                 .FirstOrDefault(v => v.Id == voucherId && v.Order.UserId == userId);

            
            if (voucher == null)
            {
                return false;
            }
            
            if (date.Date < DateTime.Now.Date)
            {
                return false;
            }

            
            if (date.Date > voucher.ExpiryDate.Date)
            {
                return false;
            }

         
            voucher.ReservationDate = date;
            voucher.Status = ReservationStatus.Pending;

            _context.Update(voucher);

            
            return _context.SaveChanges() > 0;
        }

        public bool ChangeStatus(int voucherId, ReservationStatus newStatus)
        {
            var voucher = _context.Vouchers.Find(voucherId);
            if (voucher == null) return false;

            voucher.Status = newStatus;
            _context.SaveChanges();
            return true;
        }
    }
}