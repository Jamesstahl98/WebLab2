using System.ComponentModel.DataAnnotations;

namespace WebLab2.Models;

public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
