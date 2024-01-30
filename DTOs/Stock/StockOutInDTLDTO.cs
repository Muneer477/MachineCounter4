
using SMTS.Entities;
using System;


namespace SMTS.DTOs.Stock
{

    public class StockOutInDTLDTO
    {
        public int DtlKey { get; set; }
        public int? Seq { get; set; }
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
        public int DocKey { get; set; }

        // Properties from PartDTL
        public string PartCode { get; set; }
        public string Description { get; set; }
    }

}
