namespace Inventory_Management_.NET.Dtos
{
    public class OrderDetailDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
    }
}
