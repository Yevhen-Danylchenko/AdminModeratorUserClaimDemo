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

        public int? ProductId { get; set; }
        public Product? Products { get; set; } 
    }
}
