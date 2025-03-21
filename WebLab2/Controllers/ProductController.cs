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
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ProductDto productDTO)
    {
        if (productDTO == null)
            return BadRequest("Product data is required");

        try
        {
            var createdProduct = await _productService.AddProductAsync(productDTO);
            if (createdProduct == null)
            {
                return BadRequest("Failed to create product.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var productDTOs = await _productService.GetAllProductsAsync();
            return Ok(productDTOs);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var productDTO = await _productService.GetProductByIdAsync(id);
            if (productDTO == null)
                return NotFound($"Product with ID '{id}' not found");

            return Ok(productDTO);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDTO)
    {
        if (productDTO == null)
            return BadRequest("Product data is required");

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDTO);
            if (updatedProduct == null)
                return NotFound($"Product with ID '{id}' not found");

            return Ok(updatedProduct);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
                return NotFound($"Product with ID '{id}' not found");

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}