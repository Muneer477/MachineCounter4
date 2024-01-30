using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class PIOTRunning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? IOTDeviceId { get; set; }

        public DateTime? Time { get; set; }

        public string? MachineNo { get; set; }

        public string? IpAddress { get; set; }
    }
}