using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Models.Entities
{
    [Index(nameof(Email),IsUnique =true)]
    [Index(nameof(UserName), IsUnique = true)]

    public class User
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
