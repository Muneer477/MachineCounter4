using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{
    [Keyless]
    public class StockJobOperationRelation
    {
        [ForeignKey("JobOrderOperation")]
        [Required]
        public int JobOperationId { get; set; }
        [ForeignKey("StockOutInDTL")]
        [Required]
        public int StockDtlKey { get; set; }
    }
}
