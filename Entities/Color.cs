namespace SMTS.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string ColorName { get; set; } // Changed the name from 'Color' to 'ColorName' to avoid naming conflict with the class name
        public string ColorCode { get; set; }
    }

}
