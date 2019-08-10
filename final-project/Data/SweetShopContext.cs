using final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace final_project.Data
{
    public class SweetShopContext : DbContext
    {
        public SweetShopContext(DbContextOptions<SweetShopContext> options) : base(options)
        {
        }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Costumer> Costumers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Order)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(p => p.OrderID);

        }

    }
}