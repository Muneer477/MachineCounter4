using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class PartDTLPrefixBatchSNDto
    {
        public int Id { get; set; }
        public string? BatchNo { get; set; }
        public string? Description { get; set; }
        public int? PartId { get; set; }
    }
}
