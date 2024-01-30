using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class StockJobOperationRelationDTO
    {
        public int JobOperationId { get; set; }

        public int StockDtlKey { get; set; }
    }
}
