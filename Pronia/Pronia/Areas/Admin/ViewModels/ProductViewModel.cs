using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    [Range(1,5)]
    public int Rating { get; set; }
    public string Image { get; set; }
    public int CategoryId { get; set; }
    public int[] TagIds { get; set; }
}
