using Microsoft.EntityFrameworkCore;
using WebLab2.Components.Pages;
using WebLab2.Data;
using WebLab2.Entities;
using WebLab2.Models;
using WebLab2.Repositories;

namespace WebLab2.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderItemService _orderItemService;

    public OrderService(IUnitOfWork unitOfWork, IOrderItemService orderItemService)
    {
        _unitOfWork = unitOfWork;
        _orderItemService = orderItemService;
    }

    public async Task<Order> CreateOrderAsync(OrderDto orderDto)
    {
        if (orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            throw new ArgumentException("Order must contain at least one item.");

        var order = new Order
        {
            UserId = orderDto.UserId,
            Status = "Pending",
            CreatedDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };

        await _orderItemService.AddOrderItemsAsync(order, orderDto.OrderItems);

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return order;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        return await _unitOfWork.Orders.GetAllAsync();
    }
}