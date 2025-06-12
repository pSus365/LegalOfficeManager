using LegalOfficeManagerApp.Models;
using Riok.Mapperly.Abstractions;

namespace LegalOfficeManagerApp.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        public partial ApplicationUser ToEntity(RegisterViewModel model);
    }
}
