using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Repositories;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetAllAsync();
}
