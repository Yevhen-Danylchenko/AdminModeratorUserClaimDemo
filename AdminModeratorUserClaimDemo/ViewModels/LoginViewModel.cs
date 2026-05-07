using AdminModeratorUserClaimDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace AdminModeratorUserClaimDemo.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public UserEnum IsAdmin { get; set; } = UserEnum.User;
    }
}
