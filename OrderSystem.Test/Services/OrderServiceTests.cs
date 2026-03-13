using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure;
using OrderSystem.Services;

namespace OrderSystem.Test.Services
{
    public class OrderServiceTests
    {

        [Fact]
        public async Task AddProductAsync_WhenOrderExists_AddsProductToOrder()
        {
            // 1. ARRANGE
            // Setup In-Memory Database
            var options = new DbContextOptionsBuilder<YourDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;

            using var context = new YourDbContext(options);

            // Seed an initial order
            var orderId = 1;
            var existingOrder = new Order { Id = orderId, Products = new List<Product>() };
            context.Orders.Add(existingOrder);
            await context.SaveChangesAsync();

            var service = new YourService(context); // Replace with your actual service name
            var newProduct = new Product { Id = 101, Name = "Test Product" };

            // 2. ACT
            await service.AddProductAsync(orderId, newProduct);

            // 3. ASSERT
            var updatedOrder = await context.Orders.Include(o => o.Products).FirstAsync(o => o.Id == orderId);
            Assert.Single(updatedOrder.Products);
            Assert.Contains(updatedOrder.Products, p => p.Id == 101);
        }

        [Fact]
        public async Task AddProductAsync_WhenOrderDoesNotExist_ThrowsException()
        {
            // ARRANGE
            var options = new DbContextOptionsBuilder<YourDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Exception")
                .Options;
            using var context = new YourDbContext(options);
            var service = new YourService(context);

            // ACT & ASSERT
            await Assert.ThrowsAsync<Exception>(() => service.AddProductAsync(999, new Product()));
        }
    }




}