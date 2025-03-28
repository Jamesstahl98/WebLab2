using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public interface IOrderItemService
{
    Task AddOrderItemsAsync(Order order, List<OrderItemDto> orderItems);
}
