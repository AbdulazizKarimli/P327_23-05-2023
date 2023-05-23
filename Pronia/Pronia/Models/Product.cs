using Pronia.Models.Common;

namespace Pronia.Models;

public class Product : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Rating { get; set; }
    public string Image { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
}