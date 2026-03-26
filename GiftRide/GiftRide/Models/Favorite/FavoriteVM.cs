using System.ComponentModel.DataAnnotations;

namespace GiftRide.Models.Favorites
{
    public class FavoriteVM
    {
        public int Id { get; set; } 

        [Display(Name = "ProductName")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "Picture")]
        public string Picture { get; set; } = null!;

        [Display(Name = "Price")]
        
        public decimal Price { get; set; }
    }
}