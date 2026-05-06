using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AdminModeratorUserClaimDemo.Models
{
    public class User: IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public UserEnum IsAdmin { get; set; } = UserEnum.User;

        // Навігаційна властивість: один користувач → багато продуктів
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
