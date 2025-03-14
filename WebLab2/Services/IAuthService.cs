using Microsoft.AspNetCore.Mvc;
using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
