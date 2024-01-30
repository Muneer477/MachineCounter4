using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{
    public class StockOutInProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Value { get; set; }

        [ForeignKey("StockOutInDTL")]
        [Required]
        public int  StockOutInDtlKey { get; set; }
    }
}
