namespace WebLab2.Entities;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}
