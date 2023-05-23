using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels;

public class TagViewModel
{
    [Required]
    public string Name { get; set; }
}
