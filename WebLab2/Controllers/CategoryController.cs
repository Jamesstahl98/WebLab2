using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLab2.Entities;
using WebLab2.Models;
using WebLab2.Services;

namespace WebLab2.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductController> _logger;

    public CategoryController(IUnitOfWork unitOfWork, ILogger<ProductController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var categoryDTOs = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name
        });

        return Ok(categoryDTOs);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CategoryDto categoryDTO)
    {
        if (categoryDTO == null)
            return BadRequest("Category data is required");

        try
        {
            var category = new Category
            {
                Name = categoryDTO.Name
            };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding category: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return NotFound($"Category with Id {id} not found");

        var categoryDTO = new CategoryDto
        {
            Name = category.Name
        };

        return Ok(categoryDTO);
    }
}