using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class JoBOM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("JobOrder")]
        [Required]
        public int JoId { get; set; }

        public int? LevelId { get; set; }
        [ForeignKey("PartDTL")]
        [Required]
        public int PartId { get; set; }
        public decimal? Quantity { get; set; }        
        public decimal? PlannedQty { get; set; }
    }
}
