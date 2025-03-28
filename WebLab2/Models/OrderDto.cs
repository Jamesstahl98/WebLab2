namespace WebLab2.Models;

public class OrderDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = "Pending";
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}
