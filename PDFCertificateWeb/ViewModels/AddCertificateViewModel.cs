using PDFCertificateWeb.ViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace PDFCertificateWeb.ViewModels
{
    public class AddCertificateViewModel
    {
        [Required]
        [DataType(DataType.Upload)]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pfx" })]
        public IFormFile File { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
