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

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany();

            modelBuilder.Entity<Order>()
                .Navigation(o => o.Products)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            base.OnModelCreating(modelBuilder);
        }
    }
}