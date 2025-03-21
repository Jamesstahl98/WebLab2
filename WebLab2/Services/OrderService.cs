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

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(OrderDto orderDto)
    {
        if (orderDto.Quantity <= 0)
            throw new ArgumentException("Invalid order quantity.");

        var order = new Order
        {
            ProductId = orderDto.ProductId,
            Quantity = orderDto.Quantity,
            UserId = orderDto.UserId,
            Date = orderDto.CreatedDate
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return order;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        return await _unitOfWork.Orders.GetAllAsync();
    }
}