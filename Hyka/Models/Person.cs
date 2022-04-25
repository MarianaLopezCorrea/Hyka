using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String Id { get; set; }
        [Required]
        public String DocumentType { get; set; }
        [Required]
        public String FullName { get; set; }
        [Required]
        public String Gender { get; set; }
        [Required]
        public String Department { get; set; }
        [Required]
        public String Municipality { get; set; }
        [Required]
        public String BloodType { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime RegisterDateTime { get; set; } = DateTime.Now;

        public String TariffId { get; set; }
    }

}