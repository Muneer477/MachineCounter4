namespace SMTS.DTOs
{
    public class UOMsDto
    {
        public int Id { get; set; } //Relation to 4 (BOMMaterial, JobOrderMaterial, PartDTL, PartUOM)
        public string UOM { get; set; }
        public string Type { get; set; }
    }
}
