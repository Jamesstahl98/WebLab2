namespace WebLab2.Models;

public class TokenResponseDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; } = DateTime.UtcNow.AddDays(7);
}
