using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class MyUser : IdentityUser
    {
        [MaxLength(128)]
        public string? Name { get; set; }
    }
}
