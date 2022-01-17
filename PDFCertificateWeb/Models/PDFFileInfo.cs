using Microsoft.AspNetCore.Identity;

namespace PDFCertificateWeb.Models
{
    public class PDFFileInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime UploadDate { get; set; }

        public string ?CertificateId { get; set; }
        public virtual Certificate ?Certificate { get; set; }

        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

    }
}
