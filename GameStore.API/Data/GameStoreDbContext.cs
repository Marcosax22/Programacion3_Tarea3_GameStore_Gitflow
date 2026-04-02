using GameStore.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data
{
    public class GameStoreDbContext : DbContext
    {
        public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options)
        {
        }
        public DbSet<Games> Games { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Games>().HasIndex(c => c.Name).IsUnique();

        }
    }
}
