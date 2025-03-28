using System.ComponentModel.DataAnnotations.Schema;

namespace WebLab2.Entities;

public class Order
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public User User { get; set; }
}
