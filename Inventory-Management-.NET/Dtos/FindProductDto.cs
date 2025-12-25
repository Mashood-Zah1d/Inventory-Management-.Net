namespace Inventory_Management_.NET.Dtos
{
    public class FindProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductQuantity { get; set; }

        public int ProductCategory { get; set; }
    }
}
