using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Infrastructure.Data.Entities
{
    public class FaqEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Question { get; set; } 

        [Required]
        public string? Answer { get; set; }

        [Required]
        public string? Keywords { get; set; } 
    }
}
