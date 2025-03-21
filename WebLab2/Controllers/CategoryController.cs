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
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categoryDTOs = await _categoryService.GetAllCategoriesAsync();
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
            var createdCategory = await _categoryService.AddCategoryAsync(categoryDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding category: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _categoryService.DeleteCategoryAsync(id);
        if (!isDeleted)
            return NotFound($"Category with Id {id} not found");

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDTO)
    {
        if (categoryDTO == null)
            return BadRequest("Category data is required");

        var isUpdated = await _categoryService.UpdateCategoryAsync(id, categoryDTO);
        if (!isUpdated)
            return NotFound($"Category with Id {id} not found");

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoryDTO = await _categoryService.GetCategoryByIdAsync(id);
        if (categoryDTO == null)
            return NotFound($"Category with Id {id} not found");

        return Ok(categoryDTO);
    }
}