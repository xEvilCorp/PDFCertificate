using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFCertificateConsole.Models
{
    public class CertificateInfoConfig
    {
        public string certificateStore { get; set; }
        public string certificateName { get; set; }
        public string certificatePassword { get; set; }
        public string fileId { get; set; }
    }
}
