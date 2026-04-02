using GameStore.API.Models.Entities;

namespace GameStore.API.Models.Dtos
{
    public static class GameMappings
    {
        public static GameDto ToDto(this Games g) => new GameDto
        {
            Id = g.id,
            Name = g.Name,
            Description = g.Description,
            Price = g.Price
        };

        public static Games ToEntity(this GameCreateDto d) => new Games
        {
            Name = d.Name,
            Description = d.Description,
            Price = d.Price
        };

        public static void MapToEntity(this GameUpdateDto d, Games e)
        {
            
            e.Name = d.Name;
            e.Description = d.Description;
            e.Price = d.Price;
        }
    }
}
