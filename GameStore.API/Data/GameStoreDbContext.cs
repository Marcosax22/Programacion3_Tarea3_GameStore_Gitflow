using GameStore.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data
{
    public class GameStoreDbContext : DbContext
    {
        public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Games> Games => Set<Games>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Games>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(g => g.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(g => g.Price)
                    .HasPrecision(18, 2);

                entity.HasIndex(g => g.Name)
                    .IsUnique();
            });
        }
    }
}