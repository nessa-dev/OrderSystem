using Microsoft.AspNetCore.Mvc;
using OrderSystem.Services;
using OrderSystem.Domain.Entities; 

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
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var created = await _orderService.CreateOrderAsync(order);
            return Ok(created);
        }
    }
}