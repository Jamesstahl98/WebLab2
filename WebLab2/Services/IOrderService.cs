using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(OrderDto order);
}
