using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{    
    public class JobOperationStatusDTO
    {       
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTime? Datetime { get; set; }        
        public int JobOpId { get; set; }
        public int? PlanningId { get; set; }

        //Properties get WCId from JobOrderOperation through JobOpId
        public int? WCId { get; set; }

        //Properties get OperationName from WorkCenter through WCId
        public string? OperationName { get; set; }

        //Properties get JoBOMId from JobOrderOperation through JobOpId
        public int JoBOMId { get; set; }

        //Properties get PartId from JoBOM through JoBOMId
        public int? PartId { get; set; }

    }
}
