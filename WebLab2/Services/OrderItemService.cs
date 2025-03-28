using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddOrderItemsAsync(Order order, List<OrderItemDto> orderItems)
    {
        foreach (var item in orderItems)
        {
            if (item.Quantity <= 0)
                throw new ArgumentException("Invalid order item quantity.");

            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with Id {item.ProductId} does not exist.");

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Product = product,
                Quantity = item.Quantity,
                ProductUnitPrice = product.Price,
                Order = order
            };

            // Add to the Order's items collection
            order.OrderItems.Add(orderItem);
        }
    }
}
