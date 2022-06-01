using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Tariff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public Rule Rule { get; set; }

        
        public Guid RuleId { get; set; }

        public Tariff() { }
    }

    public class Rule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public String Attribute { get; set; }

        [Required]
        public String Condition { get; set; }

        [Required]
        public String Value { get; set; }

    }

}
