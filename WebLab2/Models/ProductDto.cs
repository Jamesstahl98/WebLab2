using static WebLab2.Enums.ProductStatusEnum;

namespace WebLab2.Models;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; 
        set; }
    public ProductStatus Status { get; set; }
}
