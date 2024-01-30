using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMTS.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //use as Foreign Key in Table Contact, Job Order Report
        public string EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Remark { get; set; }
        public bool? Active { get; set; }
        public int? DepartmentId { get; set; }        
    }
}
