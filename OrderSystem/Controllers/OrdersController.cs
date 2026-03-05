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
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var created = await _orderService.CreateOrderAsync();
            return Ok(created);
        }

        [HttpPost("{id}/add-product")]
        public async Task<IActionResult> AddProduct(int id, [FromBody] Product product)
        {
            try
            {
                await _orderService.AddProductAsync(id, product);
                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/finalize")]
        public async Task<IActionResult> Finalize(int id)
        {
            try
            {
                await _orderService.FinalizeOrderAsync(id);
                return Ok("Order finalized.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}