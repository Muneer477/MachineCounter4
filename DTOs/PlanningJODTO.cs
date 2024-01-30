using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class PlanningJODTO
    {
       
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTimeOffset? EstimateStartDate { get; set; }
        public DateTimeOffset? EstimateEndDate { get; set; }
     
        public int WCId { get; set; } // [ForeignKey("WorkCenter")]
        public decimal? Qty { get; set; }
        public int? Sequence { get; set; }
        public string PlannedBy { get; set; }
       
      
        public int? JoOrderId { get; set; }// [ForeignKey("JobOrder")]

     
        public int? JoOperationId { get; set; } // [ForeignKey("JobOrderOperation")]
    }
}
