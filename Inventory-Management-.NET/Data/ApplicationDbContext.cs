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
    }
}
