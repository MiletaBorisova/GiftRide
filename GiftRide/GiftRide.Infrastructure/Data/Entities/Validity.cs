using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Infrastructure.Data.Entities
{
    public class Validity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? ValidityName { get; set; }

        [Required]
        [Range(1, 24)]
        public int MonthsCount { get; set; }

        public virtual IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
