namespace Inventory_Management_.NET.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
