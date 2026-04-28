using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AdminModeratorUserClaimDemo.Models;

namespace AdminModeratorUserClaimDemo.Data
{
    public class ApplicationDbContext: IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure the relationship between User and Product
            builder.Entity<User>()
                .HasOne(u => u.Products)
                .WithMany()
                .HasForeignKey(u => u.ProductId);
        }    
    }
}
