using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.DTOs;
using OrderSystem.Infrastructure;

namespace OrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProductDto dto)
        {
            var product = new Product(dto.Name, dto.Price);
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // READ 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // READ BY ID 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Product not found in the catalog.");

            return Ok(product);
        }

        // UPDATE 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Product not found.");

            product.Name = dto.Name;
            product.Price = dto.Price;

            await _context.SaveChangesAsync();
            return Ok("Product updated successfully.");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Product not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product removed from the catalog.");
        }
    }
}