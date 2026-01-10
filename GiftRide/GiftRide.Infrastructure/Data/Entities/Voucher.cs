using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Infrastructure.Data.Entities
{
    public enum ReservationStatus
    {
        None = 0,         
        Pending = 1,     
        Approved = 2,     
        Rejected = 3    
    }
    public class Voucher
    {
        [Required]
        public int Id { get; set; }

        
        public string VoucherCode
        {
            get => Id.ToString("D4"); 
        }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? ReservationDate { get; set; }
        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.None;
    }
}
