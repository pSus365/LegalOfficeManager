using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LegalOfficeManagerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>  // 01:46:42
    {
     
        public DbSet<Document> Documents { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<LegalOfficeEntry> LegalOfficeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
    
        }


    }


}
