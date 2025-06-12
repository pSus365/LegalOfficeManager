using System.ComponentModel.DataAnnotations;

namespace LegalOfficeManagerApp.Models
{
    public class LegalOfficeEntry
    {
        // at this moment , the only entry is a client for a starting page [the list of current clients]
        // TODO: add more entries like lawyer, case, etc. -> connected with RBAC

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty; // is it needed? -> maybe for the future, when we will have a lot of clients from different countries
        public string CaseType { get; set; } = string.Empty; // TODO: add a list of case types in the database and connect it with this field OR ENUM?
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // TODO: add a list of statuses in the database and connect it with this field OR ENUM?
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        [Display(Name = "Enable Two-Factor Authentication")]
        public bool EnableTwoFactor { get; set; }

    }
}
