using Inventory_Management_.NET.Dtos;

namespace Inventory_Management_.NET.Models
{
    public class BillViewModel
    {
        public CustomerDetailDto Customer { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
