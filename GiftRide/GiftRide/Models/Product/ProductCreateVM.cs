using GiftRide.Models.Category;
using GiftRide.Models.Validity;
using System.ComponentModel.DataAnnotations;

namespace GiftRide.Models.Product
{
    public class ProductCreateVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Validity")]
        public int ValidityId { get; set; }
        public virtual List<ValidityPairVM> Validities { get; set; } = new List<ValidityPairVM>();

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();

        [Display(Name = "Picture")]
        public string? Picture { get; set; }

        [Range(0, 5000)]
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
