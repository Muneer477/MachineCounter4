using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class UserCredentials
    {
        [Required]
        public string EmployeeID { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
