using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Services
{
    public class DashBoardService
    {
        private readonly ApplicationDbContext dbContext;

        public DashBoardService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<DashboardStatsDto> getDashboardStatsASync()
        {
            return new DashboardStatsDto
            {
                productCount = await dbContext.Products.CountAsync(),
                customerCount = await dbContext.Customers.CountAsync(),
                purchaseCount = await dbContext.purchaseHistories.CountAsync(),
                orderCount = await dbContext.Orders.CountAsync(),
                products = await dbContext.Products.ToListAsync(),
            };
        }
    }
}
