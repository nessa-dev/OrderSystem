using Microsoft.AspNetCore.Mvc;
using OrderSystem.Domain.Entities;
using OrderSystem.DTOs;
using OrderSystem.Services;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var created = await _orderService.CreateOrderAsync();
            return Ok(created);
        }

        [HttpPost("{id}/add-product")]
        public async Task<IActionResult> AddProduct(int id, [FromBody] AddProductDto dto)
        {
            var product = new Product(dto.Name, dto.Price);

            await _orderService.AddProductAsync(id, product);

            return Ok();
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

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _orderService.CancelOrderAsync(id);
            return Ok("Order cancelled.");
        }

        [HttpGet("{id}/total")]
        public async Task<IActionResult> GetTotal(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order.CalculateTotal());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
        {
            try
            {
                await _orderService.UpdateOrderAsync(id, dto.CustumerName);
                return Ok("Order updated.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return Ok("Order deleted.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}