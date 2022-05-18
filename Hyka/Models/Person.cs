using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public String Id { get; set; }

        public String DocumentType { get; set; }

        [Required]
        public String FullName { get; set; }

        [Required]
        public String Gender { get; set; }

        [Required]
        public String Country { get; set; }

        public String Department { get; set; }

        public String Municipality { get; set; }

        public String BloodType { get; set; }

        [Required]
        public int Age { get; set; }

        public DateTime RegisterDateTime { get; set; } = DateTime.Now;

        [ForeignKey("TariffId")]
        public String TariffId { get; set; }
    }

}