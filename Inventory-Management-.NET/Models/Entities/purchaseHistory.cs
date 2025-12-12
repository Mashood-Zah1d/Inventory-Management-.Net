using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_.NET.Models.Entities
{
    public class purchaseHistory
    {
        [Key]
        public Guid purchaseId { get; set; }

        public Guid ProductId { get; set; }

        public int purchaseQuantity { get; set; }

        public DateOnly PurchaseDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
