namespace Inventory_Management_.NET.Models
{
    public class PurchaseHistoryViewModel
    {

        public Guid ProductId { get; set; }

        public int purchaseQuantity { get; set; }

        public DateOnly PurchaseDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
