using Newtonsoft.Json;
using PDFCertificateConsole.Models;
using System.Security.Cryptography.X509Certificates;

namespace PDFCertificateConsole
{
    public class Certificate
    {
        public static X509Certificate2? LoadCertificate(string certificateName, string certificatePassword)
        {
            var store = new X509Store("PDFAppCertificates", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            foreach (var certificate in store.Certificates)
            {
                if (certificate.FriendlyName.Contains(certificateName))
                {
                    Console.WriteLine($"Certificate {certificateName} loaded.");

                    return certificate;
                    //var cert = new X509Certificate2(certificate.GetRawCertData(), certificatePassword);
                    //return cert;
                }
            }

            return null;
        }

        public static Stream LoadCertificateStream(X509Certificate2 cert, string password)
        {
            return new MemoryStream(cert.Export(X509ContentType.Pfx, password));
        }

        public static CertificateInfoConfig LoadConfig(string configPath)
        {
            var path = Path.Combine(configPath);

            using (StreamReader r = new StreamReader(path))
            {
                var json = r.ReadToEnd();
                var config = JsonConvert.DeserializeObject<CertificateInfoConfig>(json);
                return config;
            }
        }
    }
}
