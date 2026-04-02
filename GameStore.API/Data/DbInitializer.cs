using GameStore.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(GameStoreDbContext db)
        {
            await db.Database.EnsureCreatedAsync();

            if (await db.Games.AnyAsync())
                return;

            var games = new List<Games>
            {
                new Games
                {
                    Name = "EA Sports FC 24",
                    Description = "Football simulation game with updated teams and career mode.",
                    Price = 59.99m
                },
                new Games
                {
                    Name = "Minecraft",
                    Description = "Sandbox game focused on building, survival, and creativity.",
                    Price = 29.99m
                },
                new Games
                {
                    Name = "The Witcher 3",
                    Description = "Open world RPG with story-driven quests and monster hunting.",
                    Price = 39.99m
                },
                new Games
                {
                    Name = "Hades",
                    Description = "Roguelike action game set in the underworld.",
                    Price = 24.99m
                },
                new Games
                {
                    Name = "Celeste",
                    Description = "Precision platformer with a strong narrative and challenging levels.",
                    Price = 19.99m
                }
            };

            db.Games.AddRange(games);
            await db.SaveChangesAsync();
        }
    }
}