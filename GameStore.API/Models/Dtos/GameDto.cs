using System;
namespace GameStore.API.Models.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }           
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
