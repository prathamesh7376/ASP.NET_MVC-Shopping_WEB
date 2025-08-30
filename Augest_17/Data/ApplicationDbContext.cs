using Augest_17.Models;
using Microsoft.EntityFrameworkCore;

namespace Augest_17.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.PriceAtLike)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(l => l.User)
                      .WithMany(u => u.Likes)   // ✅ User has many Likes
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.Product)
                      .WithMany(p => p.Likes)   // ✅ Product has many Likes
                      .HasForeignKey(l => l.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.PriceAtAdd)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Product)
                      .WithMany()
                      .HasForeignKey(c => c.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
