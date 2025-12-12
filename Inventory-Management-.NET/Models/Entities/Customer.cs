namespace Inventory_Management_.NET.Models.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
