using System.ComponentModel.DataAnnotations.Schema;

namespace WebLab2.Entities;

public class Order
{
    public int Id { get; set; }
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime Date { get; set; }
}
