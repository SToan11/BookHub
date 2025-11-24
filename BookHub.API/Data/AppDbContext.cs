using BookHub.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BookHub.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Staffs
            modelBuilder.Entity<Staff>().HasData(
                new Staff
                {
                    Id = Guid.NewGuid(),
                    Username = "owner01",
                    PasswordHash = HashPassword("Abc123@@"),
                    Email = "toanadmin@gmail.com",
                    PhoneNumber = "0900000001",
                    Role = "owner"
                },
                new Staff
                {
                    Id = Guid.NewGuid(),
                    Username = "employee01",
                    PasswordHash = HashPassword("Abc123@@"),
                    Email = "toanstaff@gmail.com",
                    PhoneNumber = "0900000002",
                    Role = "employee"
                }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Username = "customer01",
                    PasswordHash = HashPassword("Abc123@@"),
                    FullName = "Nguyen Van A",
                    Email = "toankh@gmail.com",
                    PhoneNumber = "0900000003",
                    Address = "123 Le Loi, TP.HCM"
                }
            );
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

                    modelBuilder.Entity<ProductCategory>()
                        .HasOne(pc => pc.Product)
                        .WithMany(p => p.ProductCategories)
                        .HasForeignKey(pc => pc.ProductId);

                    modelBuilder.Entity<ProductCategory>()
                        .HasOne(pc => pc.Category)
                        .WithMany(c => c.ProductCategories)
                        .HasForeignKey(pc => pc.CategoryId);

        }

        // Hàm hash mật khẩu dùng chung (SHA256)
        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

    }
}
