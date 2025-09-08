using System.ComponentModel.DataAnnotations.Schema;

namespace LegalOfficeManagerApp.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public int  LegalOfficeEntryId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }

        [ForeignKey(nameof(LegalOfficeEntryId))]
        public LegalOfficeEntry LegalOfficeEntry { get; set; }
    }

}
