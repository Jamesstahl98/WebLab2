using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Security.Claims;
using WebLab2.Entities;
using WebLab2.HelperMethods;
using WebLab2.Models;
using WebLab2.Services;

namespace WebLab2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var user = await authService.RegisterAsync(request);
        if (user is null)
        {
            return BadRequest("Username already exists");
        }

        return Ok(user);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto request)
    {
        var response = await authService.LoginAsync(request);
        if (response is null)
        {
            return Unauthorized("Invalid username or password");
        }

        Response.Cookies.Append("AccessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(1)
        });

        Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = response.RefreshTokenExpiration
        });

        return Ok(new { message = "Login successful" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        //Does not work. Cookies are deleted in frontend for now.
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["RefreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Refresh token missing");
        }

        refreshToken = Uri.UnescapeDataString(refreshToken);

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");

        var response = await authService.RefreshTokensAsync(new RefreshTokenRequestDto
        {
            UserId = userId,
            RefreshToken = refreshToken
        });

        if (response is null)
        {
            return Unauthorized("Invalid refresh token");
        }

        return Ok(new { message = "Token refreshed" });
    }

    [Authorize]
    [HttpGet("authorize")]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are admin");
    }
}
