namespace WebLab2.Models;

public class OrderDto
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int ProductId { get; set; }
    public string? UserEmail { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
}
