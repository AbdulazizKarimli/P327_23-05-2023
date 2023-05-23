using Pronia.Models.Common;

namespace Pronia.Models;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Product> Products { get; set; }
}