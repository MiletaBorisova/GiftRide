using System.ComponentModel.DataAnnotations;

namespace GiftRide.Models.Product
{
    public class ProductDeleteVM
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }


        [Display(Name = "Validity")]
        public int ValidityId { get; set; }
        public string? ValidityName { get; set; }



        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }


        [Display(Name = "Picture")]
        public string? Picture { get; set; }


        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Display(Name = "Discounted Price")]
        public decimal DiscountedPrice => Price * (1 - Discount / 100m);
    }
}
