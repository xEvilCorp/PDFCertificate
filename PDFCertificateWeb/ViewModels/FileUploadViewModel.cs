using PDFCertificateWeb.ViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace PDFCertificateWeb.ViewModels
{
    public class FileUploadViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [MaxFileSize(50 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile File { get; set; }
    }
}
