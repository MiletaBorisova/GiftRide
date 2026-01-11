using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace GiftRide.Models.Category
{
    public class CategoryPairVM
    {
        public int Id { get; set; }
        [Display(Name = "Category")]
        public string? Name { get; set; }
    }
}
