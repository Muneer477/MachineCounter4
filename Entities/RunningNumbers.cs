using System.ComponentModel.DataAnnotations;

namespace SMTS.Entities
{
    public class RunningNumbers
    {
        [MaxLength(10)]
        public string Prefix { get; set; }
        public int? Number { get; set; }
    }

}
