using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.ToListAsync();

            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagViewModel tagViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            bool isExist = _context.Tags.Any(t => t.Name == tagViewModel.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Name already exist");
                return View();
            }

            Tag tag = new Tag
            {
                Name = tagViewModel.Name,
            };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
