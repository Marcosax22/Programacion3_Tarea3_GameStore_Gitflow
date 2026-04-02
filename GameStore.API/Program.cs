using GameStore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connFromEnv = Environment.GetEnvironmentVariable("GAMESTORE_CONNECTION");
            var connectionString = !string.IsNullOrWhiteSpace(connFromEnv)
                ? connFromEnv
                : builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<GameStoreDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GameStoreDbContext>();
                await DbInitializer.InitializeAsync(db);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}