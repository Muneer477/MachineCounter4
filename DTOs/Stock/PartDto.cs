using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class PartDto
    {
        public int Id { get; set; }
        public string? PartCode { get; set; }
        public string? Description { get; set; }
        public string? Product { get; set; }
        public string? Customer { get; set; }
        public string? Category { get; set; }
        public string? PartType { get; set; }
        public string? PartGroup { get; set; }
        public decimal? Qty { get; set; }       
        public int? UOMId { get; set; }
        public string? UOM { get; set; }
        public int? BOMId { get; set; }
        public string? Remark { get; set; }
        public string? Grade { get; set; }
        public string? BasedColor { get; set; }
        public int? BasedColorId { get; set; }
        public string? Thickness { get; set; }
        public string? ThicknessUOM { get; set; }
        public int? ThicknessUOMId { get; set; }
        public string? Width { get; set; }
        public string? WidthUOM { get; set; }
        public int? WidthUOMId { get; set; }
        public string? Width2 { get; set; }
        public string? Width2UOM { get; set; }
        public int? Width2UOMId { get; set; }
        public string? WidthAdditional { get; set; }
        public string? Length { get; set; }
        public string? LengthUOM { get; set; }
        public int? LengthUOMId { get; set; }
        public string? Size { get; set; }
        public bool? Opaque { get; set; }
        public string? PackingInstruction { get; set; }
        public decimal? QtyAcceptance { get; set; }
        public bool? HaveBOM { get; set; }
        public bool? BatchTracking { get; set; }
        public bool? SerialNumberTracking { get; set; }
    }
}
