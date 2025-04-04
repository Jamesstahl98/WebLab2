﻿using WebLab2.Models;

namespace WebLab2.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto);
    Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}
