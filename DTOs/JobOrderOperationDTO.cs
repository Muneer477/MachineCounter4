using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.DTOs
{
    public class JobOrderOperationDTO
    {
        public int Id { get; set; }
        public int? OpNo { get; set; }       
        public int WCId { get; set; }//[ForeignKey("WorkCenter")]
        public decimal? SetupTime { get; set; }
        public decimal? UnitTime { get; set; }
        public decimal? TotalUnitTime { get; set; }
        public string? Unit { get; set; }
        public DateTimeOffset? RunDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }        
        public int JoBOMId { get; set; }//[ForeignKey("JoBOM")]
    }
}
