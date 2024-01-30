using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class PlanningJO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTimeOffset? EstimateStartDate { get; set; }
        public DateTimeOffset? EstimateEndDate { get; set; }
        [ForeignKey("WorkCenter")]
        [Required]
        public int? WCId { get; set; }
        public decimal? Qty { get; set; }
        public int? Sequence { get; set; }
        public string? PlannedBy { get; set; }
        [ForeignKey("JobOrder")]
        [Required]
        public int? JoOrderId { get; set; }
        [ForeignKey("JobOrderOperation")]
        [Required]
        public int? JoOperationId { get; set; }
    }
}
