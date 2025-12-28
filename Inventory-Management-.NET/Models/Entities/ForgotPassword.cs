namespace Inventory_Management_.NET.Models.Entities
{
    public class ForgotPassword
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string Code { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}
