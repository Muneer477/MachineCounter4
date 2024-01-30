using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{

    public class StockOutInDTL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DtlKey { get; set; }
        public int? Seq { get; set; }
        [ForeignKey("PartDTL")]
        [Required]
        public int? PartId { get; set; }
        public int? LocationId { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
        public DateTime? Date { get; set; }
        public string? Batch { get; set; }
        public string? SerialNumber { get; set; }
        public string? Type { get; set; }
        public string? Remark { get; set; }
        public string? TypeOf { get; set; }
        [ForeignKey("StockOutIn")]
        [Required]
        public int? DocKey { get; set; }
    }

}