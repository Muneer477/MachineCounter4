using SMTS.Entities;

namespace SMTS.DTOs.Stock
{
    public class JobOrderWithOperationStatusDTO
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTime? Datetime { get; set; }        
        public int JobOpId { get; set; }
        public int? PlanningId { get; set; }
        
        //Properties from JobOrder
        public int? JoId { get; set; } //
        public string? JoNo { get; set;} //
        public int? partId { get; set; } //

    }
}
