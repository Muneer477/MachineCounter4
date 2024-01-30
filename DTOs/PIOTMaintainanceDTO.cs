using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SMTS.DTOs
{
    public class PIOTMaintainanceDTO
    {
        public int Id { get; set; }

        public string? SerialNumber { get; set; }

        public string? IpAddress { get; set; }

        public string? MachineNo { get; set; }
    }
}
