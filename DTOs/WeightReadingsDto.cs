namespace SMTS.DTOs
{
    public class WeightReadingsDto
    {
        //Id is auto generated
        public Decimal Weight { get; set; }

        public string UOM { get; set; }
        public DateTime Date { get; set; }
    }
}
