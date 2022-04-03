using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Tariff
    {           
        [Key]
        public Guid Id { get; set; }
        [Required]
        public String TariffName { get; set; }
        [Required]
        public String Price { get; set; }
    }
}
