using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities; 

namespace OrderSystem.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; } = null!;
    }
}