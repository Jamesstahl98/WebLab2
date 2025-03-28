namespace WebLab2.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal ProductUnitPrice { get; set; }


    public Order Order { get; set; }
    public Product Product { get; set; }
}
