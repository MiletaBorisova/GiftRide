using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GiftRide.Models.Cart
{
    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new List<CartItemVM>();

        [Display(Name = "AppliedPromoDiscountPercent")]
        public decimal AppliedPromoDiscountPercent { get; set; }

        [Display(Name = "SubTotal")]
        public decimal Subtotal => Items.Sum(i => i.TotalPrice);

        [Display(Name = "FinalTotal")]
        public decimal FinalTotal { get; set; }

        [Display(Name = "DiscountAmount")]
        public decimal DiscountAmount => Subtotal - FinalTotal;
    }
}