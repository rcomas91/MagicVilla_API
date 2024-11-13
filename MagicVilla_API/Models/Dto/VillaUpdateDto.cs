using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.Dto
{
    public class VillaUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]

        public string Detail { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]

        public int Ocupants { get; set; }
        [Required]

        public int MetersCuadrados { get; set; }
        public string Amenity { get; set; }
        [Required]

        public string ImageUrl { get; set; }
    }
}
