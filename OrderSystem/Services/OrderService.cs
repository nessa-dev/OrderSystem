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

        // ✅ CREATE
        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Business rule example
            if (order.TotalAmount <= 0)
                throw new ArgumentException("Total amount must be greater than zero.");

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        // ✅ GET ALL
        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // ✅ GET BY ID
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        // ✅ UPDATE
        public async Task<Order?> UpdateAsync(int id, Order updatedOrder)
        {
            var existingOrder = await _context.Orders.FindAsync(id);

            if (existingOrder == null)
                return null;

            if (updatedOrder.TotalAmount <= 0)
                throw new ArgumentException("Total amount must be greater than zero.");

            existingOrder.CustomerName = updatedOrder.CustomerName;
            existingOrder.TotalAmount = updatedOrder.TotalAmount;

            await _context.SaveChangesAsync();

            return existingOrder;
        }

        // ✅ DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}