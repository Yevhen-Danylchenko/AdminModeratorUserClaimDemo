using AdminModeratorUserClaimDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminModeratorUserClaimDemo.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Виконуємо міграції
            await context.Database.MigrateAsync();

            // --- Ролі ---
            var roles = new[] { "SuperAdmin", "Admin", "Moderator" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // --- SuperAdmin користувач ---
            var superAdminEmail = "superadmin@ukr.net";
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdminUser == null)
            {
                superAdminUser = new User
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    Name = "Super Admin"
                };

                var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Не вдалося створити SuperAdmin користувача: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }

            // --- Admin користувач ---
            var adminEmail = "admin@ukr.net";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Name = "Admin User"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Не вдалося створити Admin користувача: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // --- Moderator користувач ---
            var moderatorEmail = "moderator@ukr.net";
            var moderatorUser = await userManager.FindByEmailAsync(moderatorEmail);

            if (moderatorUser == null)
            {
                moderatorUser = new User
                {
                    UserName = moderatorEmail,
                    Email = moderatorEmail,
                    EmailConfirmed = true,
                    Name = "Moderator User"
                };

                var result = await userManager.CreateAsync(moderatorUser, "Moderator123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Не вдалося створити Moderator користувача: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(moderatorUser, "Moderator"))
            {
                await userManager.AddToRoleAsync(moderatorUser, "Moderator");
            }

            // --- Продукти ---
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Wireless Mouse", Description = "Ergonomic wireless mouse with USB receiver", Price = 25.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Mechanical Keyboard", Description = "RGB backlit mechanical keyboard with blue switches", Price = 79.50m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Gaming Headset", Description = "Surround sound headset with noise-cancelling mic", Price = 59.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "USB-C Charger", Description = "Fast charging USB-C wall adapter 65W", Price = 34.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Smartphone Stand", Description = "Adjustable aluminum stand for phones and tablets", Price = 15.00m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Portable SSD", Description = "1TB external SSD with USB 3.2", Price = 129.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Bluetooth Speaker", Description = "Waterproof portable speaker with deep bass", Price = 45.00m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Webcam HD", Description = "1080p webcam with built-in microphone", Price = 39.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Smartwatch", Description = "Fitness tracking smartwatch with heart rate monitor", Price = 199.00m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Laptop Backpack", Description = "Water-resistant backpack with padded laptop compartment", Price = 49.99m, Status = ProductStatusEnum.available, CreatedAt = DateTime.UtcNow }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}


