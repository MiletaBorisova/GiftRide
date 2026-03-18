using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Infrastructure.Data.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Phone { get; set; }

        public string? Subject { get; set; }

        [Required]
        public string? Message { get; set; }
    }
}
