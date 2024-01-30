using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class PartDTLPrefixBatchSN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? BatchNo { get; set; }

        public string? Description { get; set; }

        [ForeignKey("PartDTL")]
        [Required]
        public int? PartId { get; set; }
    }
}
