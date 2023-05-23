using Microsoft.EntityFrameworkCore;

namespace Pronia.ViewComponents;

public class ProductViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public ProductViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _context.Products.Take(8).ToListAsync();

        return View(products);
    }
}