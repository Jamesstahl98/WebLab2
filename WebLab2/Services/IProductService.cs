using WebLab2.Models;

namespace WebLab2.Services;

public interface IProductService
{
    Task<ProductDto> AddProductAsync(ProductDto productDTO);
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<ProductDto> UpdateProductAsync(int id, ProductDto productDTO);
    Task<bool> DeleteProductAsync(int id);
}
