using Pronia.Models;

namespace Pronia.ViewModels;

public class HomeViewModel
{
    public List<Slider> Sliders { get; set; }
    public List<Service> Services { get; set; }
    public List<Product> Products { get; set; }
}