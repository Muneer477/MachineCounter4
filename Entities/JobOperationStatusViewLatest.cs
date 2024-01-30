using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class JobOperationStatusViewLatest
    {        
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime? Datetime { get; set; }
        public int JobOpId { get; set; }
        public int? PlanningId { get; set; }
        public int WCId { get; set; }

        public string OperationName { get; set; }
        public int PartId { get; set; }
        public string PartCode { get; set; }   
    }
}
