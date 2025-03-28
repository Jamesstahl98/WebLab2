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
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                UserEmail = o.User.Email,
                CreatedDate = o.CreatedDate,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    ProductUnitPrice = oi.ProductUnitPrice
                }).ToList()
            })
            .ToListAsync();

        return orders;
    }
}