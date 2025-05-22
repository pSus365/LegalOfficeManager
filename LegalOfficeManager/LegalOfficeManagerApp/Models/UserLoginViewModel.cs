using System.ComponentModel.DataAnnotations;

namespace LegalOfficeManagerApp.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
