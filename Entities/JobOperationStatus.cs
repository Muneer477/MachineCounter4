using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{
    public class JobOperationStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTime? Datetime { get; set; }
        [ForeignKey("JobOrderOperation")]
        [Required]
        public int JobOpId { get; set; }
        [ForeignKey("PlanningJO")]
        //[Required]
        public int? PlanningId { get; set; }
    }
}
