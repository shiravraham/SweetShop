using final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace final_project.Data
{
    public class SweetShopContext : DbContext
    {
        public SweetShopContext(DbContextOptions<SweetShopContext> options) : base(options)
        {
        }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        }
}