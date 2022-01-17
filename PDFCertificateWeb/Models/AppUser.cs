using Microsoft.AspNetCore.Identity;

namespace PDFCertificateWeb.Models
{
    public class AppUser : IdentityUser
    {
        public string ?Code { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<PDFFileInfo> Files { get; set; }
    }
}
