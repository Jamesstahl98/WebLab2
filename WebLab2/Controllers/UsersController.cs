using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebLab2.Entities;
using WebLab2.Models;
using WebLab2.Services;

namespace WebLab2.Controllers;


[ApiController]
[Route("api/user")]
public class UsersController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var userDTOs = users.Select(u => new UserDto
        {
            Username = u.Username,
            Role = u.Role,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Country = u.Country,
            City = u.City,
            Address = u.Address
        });
        return Ok(userDTOs);
    }

    [HttpGet("get-profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var userDto = new UserDto
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Country = user.Country,
            City = user.City,
            Address = user.Address
        };

        return Ok(userDto);
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserDto updatedUser)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return NotFound("User does not exist");

        if (!string.Equals(user.Username, updatedUser.Username, StringComparison.OrdinalIgnoreCase))
        {
            var usernameExists = await _unitOfWork.Users.UsernameExistsAsync(updatedUser.Username);
            if (usernameExists)
            {
                return BadRequest("Username already exists.");
            }
        }

        user.Username = updatedUser.Username;
        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Country = updatedUser.Country;
        user.City = updatedUser.City;
        user.Address = updatedUser.Address;

        if (!string.IsNullOrEmpty(updatedUser.Password))
        {
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, updatedUser.Password);
        }

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return Ok("Profile updated successfully");
    }
}
