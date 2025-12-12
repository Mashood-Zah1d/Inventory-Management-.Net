namespace Inventory_Management_.NET.Models.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        
        public Guid CustomerId { get; set; }

        public DateOnly OrderDate { get; set; }

        public Decimal TotalAmount { get; set; }
    }
}
