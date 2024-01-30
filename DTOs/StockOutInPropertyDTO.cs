
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class StockOutInPropertyDTO
    {
      
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Value { get; set; }

        public int StockOutInDtlKey { get; set; }
    }
}
 
