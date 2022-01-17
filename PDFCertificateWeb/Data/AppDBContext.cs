using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDFCertificateWeb.Models;

namespace PDFCertificateWeb.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {

        }

        public AppDBContext()
        {

        }

        public DbSet<PDFFileInfo> PDFFiles { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

    }
}