using System.ComponentModel.DataAnnotations;

namespace LegalOfficeManagerApp.Models
{
    public class PaymentViewModel
    {
        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Code must be exactly 6 digits.")]
        public string Code { get; set; }
        public string Package { get; set; }
    }

}
