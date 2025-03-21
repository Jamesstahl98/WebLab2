using WebLab2.Repositories;

namespace WebLab2.Services;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync();
}