using GameStore.API.Models.Entities;

namespace GameStore.API.Models.Dtos
{
    public static class GameMappings
    {
        public static GameDto ToDto(this Games g) => new GameDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            Price = g.Price
        };

        public static Games ToEntity(this GameCreateDto d) => new Games
        {
            Name = NormalizeText(d.Name),
            Description = NormalizeText(d.Description),
            Price = d.Price
        };

        public static void MapToEntity(this GameUpdateDto d, Games e)
        {
            e.Name = NormalizeText(d.Name);
            e.Description = NormalizeText(d.Description);
            e.Price = d.Price;
        }

        private static string NormalizeText(string value)
        {
            return value.Trim();
        }
    }
}