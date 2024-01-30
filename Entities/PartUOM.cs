using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class PartUOM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal? Rate { get; set; }
        public int? UOMId { get; set; }
        public int? PartId { get; set; }
        public string? UOMCategory { get; set; }
    }

}
