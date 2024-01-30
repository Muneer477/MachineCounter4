using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class PartUOMDto
    {
        public int Id { get; set; }

        [Required]
        public decimal? Rate { get; set; }

        [Required]
        public int? UOMId { get; set; }

        [Required]
        public int? PartId { get; set; }

        [Required]
        public string? UOMCategory { get; set; }
    }
}
