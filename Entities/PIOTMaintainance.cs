using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class PIOTMaintenance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Assuming Id is not auto-generated
        public int Id { get; set; }

        [StringLength(50)]
        public string? SerialNumber { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }

        [StringLength(50)]
        public string? MachineNo { get; set; }


    }
}
