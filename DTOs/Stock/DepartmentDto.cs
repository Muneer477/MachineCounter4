using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }      
        public string Department { get; set; }
        public bool? Active { get; set; }
    }
}
