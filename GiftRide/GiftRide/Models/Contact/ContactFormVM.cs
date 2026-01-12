using System.ComponentModel.DataAnnotations;

namespace GiftRide.Models.Contact
{
    public class ContactFormVM
    {
        [Required(ErrorMessage = "Моля, въведете име.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Моля, въведете имейл.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля, посочете телефон.")]
        public string Phone { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Съобщението не може да е празно.")]
        [MinLength(10, ErrorMessage = "Съобщението трябва да е поне 10 символа.")]
        public string Message { get; set; }
    }
}
