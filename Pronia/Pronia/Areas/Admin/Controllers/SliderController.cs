using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();

            ViewBag.Count = sliders.Count;

            return View(sliders);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
                return View();

            if (slider.Offer > 100)
            {
                ModelState.AddModelError("Offer", "Offer 100-den cox ola bilmez");
                return View();
            }

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Detail(int id)
        {
            Slider? slider = _context.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (_context.Sliders.Count() == 1)
                return BadRequest();

            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSlider(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Slider slider, int id)
        {
            Slider? dbSlider = _context.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            //dbSlider.Title = slider.Title;
            //dbSlider.Description = slider.Description;
            //dbSlider.Image = slider.Image;
            //dbSlider.Offer = slider.Offer;

            _context.Sliders.Update(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
