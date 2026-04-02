using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Models.Dtos
{
    public class GameUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Range(0, 999999.99)]
        public decimal Price { get; set; }
    }
}
