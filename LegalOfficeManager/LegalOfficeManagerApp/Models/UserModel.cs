using System.ComponentModel.DataAnnotations;

namespace LegalOfficeManagerApp.Models
{
    public class UserModel
    {

        [Key]  // opcjonalne, ale dobrze mieć
        public int Id { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
    }
}
