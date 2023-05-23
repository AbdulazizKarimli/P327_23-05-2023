using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Utils;
using Pronia.Utils.Enums;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private int _count = 0;

        public ServiceController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            IEnumerable<Service> services = _appDbContext.Services.AsEnumerable();
            _count = services.Count();
        }

        public IActionResult Index()
        {
            IEnumerable<Service> services = _appDbContext.Services.AsEnumerable();

            ViewBag.Count = _count;

            return View(services);
        }

        public IActionResult Create()
        {
            if (_count == 3)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel serviceViewModel)
        {
            if (_count == 3)
                return BadRequest();

            if (!ModelState.IsValid) return View();

            if (serviceViewModel.Image == null)
            {
                ModelState.AddModelError("Image", "Image bosh ola bilmez");
                return View();
            }
            if (!serviceViewModel.Image.CheckFileSize(100))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
                return View();
            }
            if (!serviceViewModel.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi shekil olmalidir.");
                return View();
            }

            //bool isExist = await _appDbContext.Services.AnyAsync(s => s.Name == serviceViewModel.Name);
            if (await _appDbContext.Services.AnyAsync(s => s.Name == serviceViewModel.Name))
            {
                ModelState.AddModelError("Name", "Hal hazirda bu adda bir service movcuddur");
                return View();
            }

            //string path = _webHostEnvironment.WebRootPath + @"\assets\images\website-images\" + serviceViewModel.Image.FileName;
            string fileName = $"{Guid.NewGuid()}-{serviceViewModel.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await serviceViewModel.Image.CopyToAsync(stream);
            }

            Service service = new()
            {
                Name = serviceViewModel.Name,
                Description = serviceViewModel.Description,
                Image = fileName
            };

            await _appDbContext.Services.AddAsync(service);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            //return Content(serviceViewModel.Image.FileName);
            //return Content(serviceViewModel.Image.Length.ToString());
            //return Content(serviceViewModel.Image.ContentType);
        }

        public async Task<IActionResult> Update(int id)
        {
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            ServiceViewModel serviceViewModel = new()
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description
            };

            return View(serviceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ServiceViewModel serviceViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            if(serviceViewModel.Image != null)
            {
                if (!serviceViewModel.Image.CheckFileSize(100))
                {
                    ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
                    return View();
                }
                if (!serviceViewModel.Image.CheckFileType(ContentType.image.ToString()))
                {
                    ModelState.AddModelError("Image", "Faylin tipi shekil olmalidir.");
                    return View();
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", service.Image);
                FileService.DeleteFile(path);

                string fileName = $"{Guid.NewGuid()}-{serviceViewModel.Image.FileName}";
                var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await serviceViewModel.Image.CopyToAsync(stream);
                }
                service.Image = fileName;
            }

            service.Name = serviceViewModel.Name;
            service.Description = serviceViewModel.Description;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            //Service newService = new()
            //{
            //    Id = serviceViewModel.Id,
            //    Name = serviceViewModel.Name,
            //    Description = serviceViewModel.Description,
            //};

            //_appDbContext.Services.Update(newService);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null)
                return NotFound();

            //string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", service.Image);
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}
            FileService.DeleteFile(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", service.Image);

            _appDbContext.Services.Remove(service);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
