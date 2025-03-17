using WebLab2.Entities;

namespace WebLab2.Services;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetByNameAsync(string name);
}
