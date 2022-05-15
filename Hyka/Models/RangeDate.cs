using System.ComponentModel.DataAnnotations;

namespace Hyka.Models
{
    public class RangeDate
    {
        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

    }
}