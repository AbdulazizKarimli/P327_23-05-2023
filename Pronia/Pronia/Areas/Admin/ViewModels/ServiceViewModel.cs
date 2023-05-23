using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels;

public class ServiceViewModel
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(200), Required]
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}