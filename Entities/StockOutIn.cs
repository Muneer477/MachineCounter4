using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{
    public class StockOutIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocKey { get; set; }
        [Required]
        public string DocNo { get; set; }
        public string? Desc { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
    }
}
