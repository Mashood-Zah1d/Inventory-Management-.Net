using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Services
{
    public class CustomerServices
    {
        private readonly ApplicationDbContext dbContext;

        public CustomerServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CustomerDetailDto>> GetAllCustomersAsync()
        {
            return await dbContext.Customers
                .Select(c => new CustomerDetailDto
                {
                    CustomerName = c.CustomerName,
                    Email = c.Email,
                    Phone = c.Phone
                })
                .ToListAsync();
        }
    }
}
