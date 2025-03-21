using WebLab2.Entities;

namespace WebLab2.Services;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid userId);
    Task<bool> UsernameExistsAsync(string username);
}
