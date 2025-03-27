using WebLab2.Models;

namespace WebLab2.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserProfileAsync(Guid userId);
    Task<UserDto> UpdateUserProfileAsync(Guid userId, UserDto updatedUser);
}
