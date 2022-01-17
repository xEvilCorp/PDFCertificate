using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography.X509Certificates;

namespace PDFCertificateConsole
{
    public class Signer
    {
        public static void Sign(X509Certificate2 certificate, string inputPDF, string outputPDF)
        {

            Util.CreateFolder(new DirectoryInfo(outputPDF).Parent.FullName);

            Console.WriteLine("Acquiring certificate private key.");
            var privateKey = certificate.PrivateKey;
            var akp = DotNetUtilities.GetKeyPair(privateKey).Private;
            var parameters = akp as RsaPrivateCrtKeyParameters;
            var pks = new PrivateKeySignature(parameters, DigestAlgorithms.SHA256);

            Console.WriteLine("Generating certificate object.");
            var bcCert = DotNetUtilities.FromX509Certificate(certificate);
            var chain = new Org.BouncyCastle.X509.X509Certificate[] { bcCert };

            Console.WriteLine($"Reading {inputPDF}");
            var reader = new PdfReader(inputPDF);
            Console.WriteLine($"Creating {outputPDF}");
            var os = new FileStream(outputPDF, FileMode.Create);

            Console.WriteLine("Generating certificate stamp.");
            var stamper = PdfStamper.CreateSignature(reader, os, '\0');
            var appearance = stamper.SignatureAppearance;

            var rect = new Rectangle(5, 5, 400, 60);
            rect.BackgroundColor = new BaseColor(146, 235, 138, 30);
            rect.BorderColor = new BaseColor(80, 158, 73, 30);
            rect.BorderWidth = 1;

            appearance.SetVisibleSignature(rect, 1, "first");


            Console.WriteLine($"Signing PDF {outputPDF} with the certificate...");
            MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);
            Console.WriteLine($"PDF File Successfully signed.");
        }
    }
}
