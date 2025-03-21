using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> AddProductAsync(ProductDto productDTO)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(productDTO.CategoryId);
        if (category == null)
            return null; // Or handle error accordingly

        var product = new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            CategoryId = category.Id,
            Category = category,
            Status = productDTO.Status
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        productDTO.Id = product.Id;
        return productDTO;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            Status = p.Status
        }).ToList();
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.Category?.Id ?? 0,
            Status = product.Status
        };
    }

    public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDTO)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return null;

        var category = await _unitOfWork.Categories.GetByIdAsync(productDTO.CategoryId);
        if (category == null)
            return null;

        product.Name = productDTO.Name;
        product.Description = productDTO.Description;
        product.Price = productDTO.Price;
        product.CategoryId = category.Id;
        product.Category = category;
        product.Status = productDTO.Status;

        await _unitOfWork.SaveChangesAsync();

        return productDTO;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return false;

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}