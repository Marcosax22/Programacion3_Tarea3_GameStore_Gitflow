using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Models.Entities
{
    public class Games
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} must have no more than {1} characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(500, ErrorMessage = "The field {0} must have no more than {1} characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }
    }
}