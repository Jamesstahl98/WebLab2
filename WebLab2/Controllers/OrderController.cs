using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebLab2.Data;
using WebLab2.Entities;
using WebLab2.Models;
using WebLab2.Services;

namespace WebLab2.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseItem([FromBody] OrderDto orderDto)
    {
        try
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDto);
            return StatusCode(201, new { message = "Purchase successful", orderId = createdOrder.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var ordersDto = await _orderService.GetOrdersAsync();
        return Ok(ordersDto);
    }
}