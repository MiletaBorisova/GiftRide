using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Infrastructure.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? ProductName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [Required]
        public int ValidityId { get; set; }
        public virtual Validity? Validity { get; set; }

        public string? Picture { get; set; }

        [Range(0, 5000)]
        public int Quantity { get; set; }

       // [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
