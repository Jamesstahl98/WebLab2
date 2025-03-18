using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLab2.Entities;
using WebLab2.Models;
using WebLab2.Services;

namespace WebLab2.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ProductDto productDTO)
    {
        if (productDTO == null)
            return BadRequest("Product data is required");

        try
        {
            var category = await _unitOfWork.Categories
                .GetByIdAsync(productDTO.CategoryId);

            if (category == null)
                return BadRequest($"Category with ID '{productDTO.CategoryId}' not found");

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

            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, productDTO);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _unitOfWork.Products.GetAllAsync();

        var productDTOs = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            Status = p.Status
        }).ToList();

        return Ok(productDTOs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return NotFound($"Product with ID '{id}' not found");
        var productDTO = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.Category?.Id ?? 0,
            Status = product.Status
        };
        return Ok(productDTO);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDTO)
    {
        if (productDTO == null)
            return BadRequest("Product data is required");
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return NotFound($"Product with ID '{id}' not found");
        var category = await _unitOfWork.Categories.GetByIdAsync(productDTO.CategoryId);
        if (category == null)
            return BadRequest($"Category with ID '{productDTO.CategoryId}' not found");

        product.Name = productDTO.Name;
        product.Description = productDTO.Description;
        product.Price = productDTO.Price;
        product.CategoryId = category.Id;
        product.Category = category;
        product.Status = productDTO.Status;
        await _unitOfWork.SaveChangesAsync();
        return Ok(productDTO);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return NotFound($"Product with ID '{id}' not found");
        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }
}