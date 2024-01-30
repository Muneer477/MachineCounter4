
namespace SMTS.DTOs
{
    public class JobOrderDto
    {
        public int Id { get; set; } // Id should not be provided as it is auto-generated.");
        public string? JoNo { get; set; }
        public int? PartId { get; set; }        
        public DateTime? SoDate { get; set; }
        public DateTime? RunDate { get; set; }
        public string? JoType { get; set; }
        public string? status { get; set; }        
    }
}



