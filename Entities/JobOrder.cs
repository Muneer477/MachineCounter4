using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class JobOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        public string? JoNo { get; set; }
        public int? PartId { get; set; }
        public DateTime? SoDate { get; set; }
        public DateTime? RunDate { get; set; }        
        public string? JoType { get; set; }
        public string? status { get; set; }

    }
}
