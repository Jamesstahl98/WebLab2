using Microsoft.EntityFrameworkCore;
using WebLab2.Data;
using WebLab2.Entities;

namespace WebLab2.Services;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<Category> GetByNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }
}
