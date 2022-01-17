using System.Security.Cryptography.X509Certificates;

namespace PDFCertificateWeb.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IConfiguration configuration;

        public CertificateService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetCertificateStore()
        {
            return configuration.GetValue<string>("CertificateStore");
        }

        public string AddCertificateToSystem(IFormFile certificateFile, string password, string name)
        {
            try
            {
                byte[]? fileBytes = null;
                using (var memoryStream = new MemoryStream())
                {
                    certificateFile.OpenReadStream().CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                X509Certificate2 certificate = new X509Certificate2(fileBytes, password, X509KeyStorageFlags.Exportable);
                certificate.FriendlyName = name;

                string storeName = GetCertificateStore();

                //if(GetCertificatePurposes(certificate).Contains("Document Signing"))

                using (X509Store store = new X509Store(storeName, StoreLocation.LocalMachine))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(certificate);
                }

                return "";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }



        }
    }
}
