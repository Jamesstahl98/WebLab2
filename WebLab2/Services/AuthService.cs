using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebLab2.Data;
using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration) : IAuthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IConfiguration _configuration = configuration;

    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (user is null)
        {
            return null;
        }

        var passwordHasher = new PasswordHasher<User>();
        if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return await CreateTokenResponseAsync(user);
    }

    public async Task<User?> RegisterAsync(UserDto request)
    {
        if (await _unitOfWork.Users.UsernameExistsAsync(request.Username))
        {
            return null;
        }

        var user = new User();
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

        user.Username = request.Username;
        user.PasswordHash = hashedPassword;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    private async Task<TokenResponseDto> CreateTokenResponseAsync(User user)
    {
        return new TokenResponseDto
        {
            AccessToken = GenerateToken(user),
            RefreshToken = await GenerateAndRefreshTokenAsync(user)
        };
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token"))
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
            audience: _configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        await _unitOfWork.SaveChangesAsync();
        return refreshToken;
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user is null
            || user.RefreshToken != refreshToken
            || user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            return null;
        }
        return user;
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
        {
            return null;
        }
        return await CreateTokenResponseAsync(user);
    }
}