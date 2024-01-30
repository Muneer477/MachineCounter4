using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.DTOs.Types
{
    public class TypesDTO
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public bool Active { get; set; }
    }
}
