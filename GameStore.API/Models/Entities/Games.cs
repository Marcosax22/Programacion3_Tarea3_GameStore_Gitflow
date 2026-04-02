using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.API.Models.Entities
{
    public class Games
    {
        public int id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must have no more than {1} characters.")]
        public string Name { get; set; } = null!;
        [MaxLength(500, ErrorMessage = "The field {0} must have no more than {1} characters.")]
        public string Description { get; set; } = null!;

        [Column (TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }
    }
}
