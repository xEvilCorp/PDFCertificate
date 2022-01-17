using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDFCertificateWeb.Data;
using PDFCertificateWeb.Models;
using PDFCertificateWeb.Services;
using PDFCertificateWeb.ViewModels;
using System.Security.Cryptography.X509Certificates;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PDFCertificateWeb.Controllers
{
    public class CertificateController : Controller
    {
        private readonly AppDBContext appDBContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICertificateService certificateService;

        public CertificateController(AppDBContext appDBContext, UserManager<AppUser> userManager, ICertificateService certificateService)
        {
            this.appDBContext = appDBContext;
            this._userManager = userManager;
            this.certificateService = certificateService;
        }

        [HttpGet]
        public async Task<ViewResult> Index()
        {
            ViewBag.CertificateList = await appDBContext.Certificates.ToListAsync(); ;
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add()
        {
            var user = await _userManager.GetUserAsync(User);
            var certificate = appDBContext.Certificates.ToList().FirstOrDefault(x => x.AppUserId == user.Id);
            if(certificate != null)
            ViewBag.Certificate = new { Date = certificate.UploadDate, Id = certificate.Id };
            return View();
        }

        List<string> GetCertificatePurposes(X509Certificate2 certificate)
        {
            List<string> purposes = new List<string>();
            foreach (var ext in certificate.Extensions)
            {
                var eku = ext as X509EnhancedKeyUsageExtension;
                if (eku != null)
                {
                    foreach (var oid in eku.EnhancedKeyUsages)
                    {
                        purposes.Add(oid.FriendlyName);
                    }
                }
            }
            return purposes;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddCertificateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var certId = Guid.NewGuid().ToString();
                var userId = (await _userManager.GetUserAsync(User)).Id;


                var result = certificateService.AddCertificateToSystem(model.File, model.Password, certId);
                if(result != "")
                {
                    ModelState.AddModelError("Certificate Error", result);
                }
                else
                {
                    Certificate certificate = new Certificate
                    {
                        Id = certId,
                        UploadDate = DateTime.Now,
                        AppUserId = userId,
                        Password = model.Password,
                        Location = certificateService.GetCertificateStore()
                    };

                    appDBContext.Certificates.Add(certificate);
                    appDBContext.SaveChanges();
                    return RedirectToAction("add");
                }
            }
            return View(model);

        }
    }
}
