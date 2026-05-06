using AdminModeratorUserClaimDemo.Models;

namespace AdminModeratorUserClaimDemo.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public UserEnum IsAdmin { get; set; }
        public string ProductNames { get; set; }
    }
}
