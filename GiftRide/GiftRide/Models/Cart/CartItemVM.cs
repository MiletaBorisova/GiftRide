using System.ComponentModel.DataAnnotations;

namespace GiftRide.Models.Cart
{
    public class CartItemVM
    {
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "Picture")]
        public string Picture { get; set; } = null!;

        [Display(Name = "Quantity")]
        [Range(1, 100, ErrorMessage = "Количеството трябва да е между 1 и 100.")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        
        public decimal Price { get; set; }

        [Display(Name = "TotalPrice")]
        public decimal TotalPrice => Price * Quantity;
    }
}