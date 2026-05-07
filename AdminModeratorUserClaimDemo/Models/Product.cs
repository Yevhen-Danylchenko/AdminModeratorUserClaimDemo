namespace AdminModeratorUserClaimDemo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductStatusEnum Status { get; set; } = ProductStatusEnum.available;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Зовнішній ключ до користувача
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
