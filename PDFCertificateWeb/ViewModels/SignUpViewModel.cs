using System.ComponentModel.DataAnnotations;

namespace PDFCertificateWeb.ViewModels
{
    public class SignUpViewModel
    { 

        [Required]
        [DataType(DataType.Text)]
        public string ?Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ?Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage= "Passwords do not match.")]
        public string ?ConfirmPassword { get; set; }
    }
}
