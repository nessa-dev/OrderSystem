using Microsoft.AspNetCore.Mvc;
using OrderSystem.Services;
using OrderSystem.DTOs;

namespace OrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_orderService.GetOrders());
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
        {
            var order = _orderService.CreateOrder(dto);
            return Ok(order);
        }
    }
}