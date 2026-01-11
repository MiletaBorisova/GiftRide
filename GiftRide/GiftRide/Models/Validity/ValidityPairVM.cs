using System.ComponentModel.DataAnnotations;

namespace GiftRide.Models.Validity
{
    public class ValidityPairVM
    {
        public int Id { get; set; }
        [Display(Name = "Validity")]
        public string? Name { get; set; }
    }
}
