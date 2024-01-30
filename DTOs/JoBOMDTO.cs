using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.DTOs
{
    public class JoBOMDTO
    {
        public int Id { get; set; }
        
        public int JoId { get; set; }//[ForeignKey("JobOrder")]

        public int? LevelId { get; set; }
        
        public int PartId { get; set; }//[ForeignKey("PartDTL")]
        public decimal? Quantity { get; set; }
        public decimal? PlannedQty { get; set; }
    }
}
