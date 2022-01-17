using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDFCertificateWeb.Data;
using PDFCertificateWeb.Models;
using PDFCertificateWeb.Services;
using PDFCertificateWeb.ViewModels;
using System.Text.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PDFCertificateWeb.Controllers
{
    public class FilesController : Controller
    {
        private readonly AppDBContext appDBContext;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<AppUser> userManager;
        private readonly ICertificateService certificateService;

        public FilesController(AppDBContext appDBContext, IHostingEnvironment hostingEnvironment, UserManager<AppUser> userManager, ICertificateService certificateService)
        {
            this.appDBContext = appDBContext;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            this.certificateService = certificateService;
        }

        [HttpGet]
        [Authorize]
        public ViewResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ViewResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var certificate = appDBContext.Certificates.ToList().FirstOrDefault(x => x.AppUserId == user.Id);
            ViewBag.HasCertificate = certificate != null;
            ViewBag.FilesList = (await appDBContext.PDFFiles.ToListAsync()).OrderByDescending(x => x.UploadDate).ToList(); ;
            return View();
        }

        private void addConfigToTempFolder(string fileId, string path, Certificate cert)
        {
            string fullpath = Path.Combine(path,$"{fileId}.json");

            dynamic config = new
            {
                certificateStore = certificateService.GetCertificateStore(),
                certificateName = cert.Id,
                certificatePassword = cert.Password,
                fileId = fileId
            };

            string json = JsonSerializer.Serialize(config);
            System.IO.File.WriteAllText(fullpath, json);
        }

        private void addFileToTempFolder(IFormFile file, string name, string folder)
        {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string filePath = Path.Combine(folder, name);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(FileUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = model.File;
                var user = await userManager.GetUserAsync(User);
                var certificate = appDBContext.Certificates.ToList().FirstOrDefault(x => x.AppUserId == user.Id);
                if (certificate != null)
                {
                    string rootpath = hostingEnvironment.WebRootPath;
                    string temppath = Path.Combine(rootpath, "tempfiles/" + user.Id);
                    string fileId = Guid.NewGuid().ToString();
                    string uniqueFileName = $"{fileId}.pdf";

                    addFileToTempFolder(model.File, uniqueFileName, temppath);
                    addConfigToTempFolder(fileId, temppath, certificate);

                    PDFFileInfo newFile = new PDFFileInfo
                    {
                        Id = fileId,
                        Name = model.Name,
                        UserId = user.Id,
                        CertificateId = certificate.Id,
                        Path = "files/" + user.Id + "/" + uniqueFileName,
                        UploadDate = DateTime.Now
                    };

                    appDBContext.PDFFiles.Add(newFile);
                    appDBContext.SaveChanges();
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError("Certificate", "Certificate not found.");
                }

            }

            return View(model);
        }

    }
}
