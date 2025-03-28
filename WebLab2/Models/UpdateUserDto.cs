using System.ComponentModel.DataAnnotations;

namespace WebLab2.Models;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Current password is required.")]
    public string CurrentPassword { get; set; }
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? NewPassword { get; set; } //Optional
}
