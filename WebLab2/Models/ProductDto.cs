using System.ComponentModel.DataAnnotations;
using static WebLab2.Enums.ProductStatusEnum;

namespace WebLab2.Models;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Range(0.01, 10000)]
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public ProductStatus Status { get; set; }
}
