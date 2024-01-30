namespace SMTS.Entities
{
    public class WeightReadings
    {
        public int Id { get; set; }
        public Decimal Weight { get; set; }

        public string UOM { get; set; }
        public DateTime Date { get; set; }
    }
}
