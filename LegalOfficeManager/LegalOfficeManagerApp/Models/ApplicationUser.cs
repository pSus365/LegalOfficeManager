using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LegalOfficeManagerApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }   
        public string Gender { get; set; }
    }
}
