using Inventory_Management_.NET.Models.Entities;

namespace Inventory_Management_.NET.Dtos
{
    public class DashboardStatsDto
    {
        public int productCount { get; set; }

        public int orderCount { get; set; }

        public int purchaseCount { get; set; }

        public int customerCount { get; set; }

        public List<Product> products { get; set; }
    }
}
