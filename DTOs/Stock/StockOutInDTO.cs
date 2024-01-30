using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs.Stock
{
    public class StockOutInDTO
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public string? Desc { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
    }
}
