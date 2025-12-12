using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_.NET.Models.Entities
{
    public class OrderDetails
    {
        [Key]
        public Guid OrderDetailId {  get; set; }
        
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public Decimal Price { get; set; }

        public Decimal TotalPrice { get; set; }
    }
}

