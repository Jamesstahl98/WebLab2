using Microsoft.EntityFrameworkCore;
using WebLab2.Data;
using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }
    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Join(_context.Users,
                order => order.UserId,
                user => user.Id,
                (order, user) => new
                {
                    order.Id,
                    order.UserId,
                    UserEmail = user.Email,
                    order.ProductId,
                    order.Quantity,
                    order.Date
                })
            .Join(_context.Products,
                order => order.ProductId,
                product => product.Id,
                (order, product) => new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    UserEmail = order.UserEmail,
                    ProductId = order.ProductId,
                    ProductName = product.Name,
                    Quantity = order.Quantity,
                    CreatedDate = order.Date
                })
            .ToListAsync();

        return orders;
    }
}
