namespace Inventory_Management_.NET.Models.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductQuantity { get; set; }

        public Guid CategoryId { get; set; }   
        public Category Category { get; set; }

        public string? ImageUrl { get; set; }
    }
}
