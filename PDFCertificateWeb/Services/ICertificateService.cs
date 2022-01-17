
namespace PDFCertificateWeb.Services
{
    public interface ICertificateService
    {
        string AddCertificateToSystem(IFormFile certificateFile, string password, string name);
        string GetCertificateStore();
    }
}