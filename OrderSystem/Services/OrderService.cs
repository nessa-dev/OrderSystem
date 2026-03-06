using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure;

namespace OrderSystem.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync()
        {
            var order = new Order();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Products)
                .ToListAsync();
        }

        public async Task AddProductAsync(int orderId, Product product)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found.");

            order.AddProduct(product);

            await _context.SaveChangesAsync();
        }

        public async Task FinalizeOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found.");

            order.FinalizeOrder();

            await _context.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Cancel();

            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductAsync(int orderId, int productId)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found.");

            order.RemoveProduct(productId);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new Exception("Order not found.");

            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
        }
    }
}