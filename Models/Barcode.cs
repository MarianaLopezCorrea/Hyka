using System.ComponentModel.DataAnnotations;

namespace Hyka.Models
{
    public class Barcode
    {
        [Required]
        public String Code { get; set; }
    }
}