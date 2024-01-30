using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{
    public class JobOrderOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }
        public int? OpNo { get; set; }
        [ForeignKey("WorkCenter")]
        [Required]
        public int WCId { get; set; }
        public decimal? SetupTime { get; set; }
        public decimal? UnitTime { get; set; }
        public decimal? TotalUnitTime { get; set; }
        public string Unit { get; set; }
        public DateTimeOffset? RunDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        [ForeignKey("JoBOM")]
        [Required]
        public int? JoBOMId { get; set; }
    }
}
