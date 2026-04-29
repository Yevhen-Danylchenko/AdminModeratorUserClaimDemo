using AdminModeratorUserClaimDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminModeratorUserClaimDemo.Data
{
    public static class DbInitializer
    {
        public static async Task SeedSuperAdminAsync(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1. Створюємо роль SuperAdmin, якщо її немає
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            // 2. Створюємо користувача SuperAdmin, якщо його немає
            var superAdminEmail = "superadmin@ukr.net";
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdminUser == null)
            {
                superAdminUser = new User
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Не вдалося створити SuperAdmin користувача: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // 3. Призначаємо роль SuperAdmin
            if (!await userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
        public static void Seed(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Check if there are any products already in the database
            if (context.Products.Any())
            {
                return; // Database has been seeded
            }

            // Seed initial products
            var products = new List<Models.Product>
            {
                new Product { Id = 1, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse with USB receiver", Price = 25.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 2, Name = "Mechanical Keyboard", Description = "RGB backlit mechanical keyboard with blue switches", Price = 79.50m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 3, Name = "Gaming Headset", Description = "Surround sound headset with noise-cancelling mic", Price = 59.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 4, Name = "USB-C Charger", Description = "Fast charging USB-C wall adapter 65W", Price = 34.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 5, Name = "Smartphone Stand", Description = "Adjustable aluminum stand for phones and tablets", Price = 15.00m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 6, Name = "Portable SSD", Description = "1TB external SSD with USB 3.2", Price = 129.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 7, Name = "Bluetooth Speaker", Description = "Waterproof portable speaker with deep bass", Price = 45.00m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 8, Name = "Webcam HD", Description = "1080p webcam with built-in microphone", Price = 39.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 9, Name = "Smartwatch", Description = "Fitness tracking smartwatch with heart rate monitor", Price = 199.00m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                new Product { Id = 10, Name = "Laptop Backpack", Description = "Water-resistant backpack with padded laptop compartment", Price = 49.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
