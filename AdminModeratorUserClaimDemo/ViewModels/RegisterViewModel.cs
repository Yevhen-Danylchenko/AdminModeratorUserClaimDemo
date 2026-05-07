using AdminModeratorUserClaimDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace AdminModeratorUserClaimDemo.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserEnum IsAdmin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
