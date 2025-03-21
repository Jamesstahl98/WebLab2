﻿using Microsoft.AspNetCore.Authorization;
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
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userDTOs = await _userService.GetAllUsersAsync();
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

        var userDto = await _userService.GetUserProfileAsync(userId);
        if (userDto == null) return NotFound("User not found");

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

        var updatedUserDto = await _userService.UpdateUserProfileAsync(userId, updatedUser);
        if (updatedUserDto == null)
        {
            return BadRequest("Username already exists or user not found.");
        }

        return Ok(updatedUserDto);
    }
}