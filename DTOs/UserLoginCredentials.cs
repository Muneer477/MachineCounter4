using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class UserLoginCredentials
    {
        [Required]
        public string EmployeeID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
