namespace Inventory_Management_.NET.Dtos
{
    public class AddProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductQuantity { get; set; }

        public Guid ProductCategory { get; set; }

        public string Image { get; set; }

    }
}
