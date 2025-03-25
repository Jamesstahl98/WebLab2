﻿using System.ComponentModel.DataAnnotations;

namespace WebLab2.Models;

public class UserDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Phone number is required.")]
    public string? PhoneNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; } = string.Empty;
    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = string.Empty;
    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = string.Empty;
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
