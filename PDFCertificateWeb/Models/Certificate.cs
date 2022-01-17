using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDFCertificateWeb.Models
{
    public class Certificate
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string ?Location { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public DateTime UploadDate { get; set; }

        public Certificate()
        {

        }
    }
}
