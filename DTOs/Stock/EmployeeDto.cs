namespace SMTS.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool? Active { get; set; }
        public int? DepartmentId { get; set; }
    }
}
