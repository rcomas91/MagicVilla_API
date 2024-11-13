using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Detail { get; set; }
        [Required]
        public double Price { get; set; }
        public int Ocupants { get; set; }
        public int MetersCuadrados { get; set; }
        public string Amenity { get; set; }
        public string ImageUrl { get; set; }
    }
}
