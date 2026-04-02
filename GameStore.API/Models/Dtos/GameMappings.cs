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
            Name = d.Name.Trim(),
            Description = d.Description.Trim(),
            Price = d.Price
        };

        public static void MapToEntity(this GameUpdateDto d, Games e)
        {
            e.Name = d.Name.Trim();
            e.Description = d.Description.Trim();
            e.Price = d.Price;
        }
    }
}