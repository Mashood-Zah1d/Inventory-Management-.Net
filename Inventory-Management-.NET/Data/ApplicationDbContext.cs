using Inventory_Management_.NET.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Inventory_Management_.NET.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<purchaseHistory> purchaseHistories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> orderDetails { get; set; }

        public DbSet<Customer> Customers { get; set; }
        }
}
