using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class PIOT_Counter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? IOTDeviceId { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string? MachineNo { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }

        // If the foreign key is indeed intended to be self-referential:

    }
}
