using final_project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final_project.Data
{
    public class SweetShopContext : DbContext
    {
        public SweetShopContext(DbContextOptions<SweetShopContext> options): base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            //modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<User>().HasKey("Id");
            modelBuilder.Entity<User>().
            modelBuilder.Entity<UserType>().HasKey("Id");
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}