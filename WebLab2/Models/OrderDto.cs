namespace WebLab2.Models;

public class OrderDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
}
