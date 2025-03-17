using System.ComponentModel.DataAnnotations.Schema;
using static WebLab2.Enums.ProductStatusEnum;

namespace WebLab2.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public ProductStatus Status { get; set; }
}
