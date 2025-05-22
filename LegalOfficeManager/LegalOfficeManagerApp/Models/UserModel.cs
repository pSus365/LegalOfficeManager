using System.ComponentModel.DataAnnotations;

namespace LegalOfficeManagerApp.Models
{
    public class UserModel
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is valid")]
        [EmailAddress(ErrorMessage = "Please, enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is valid")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
    }
}
