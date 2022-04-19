using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String PersonId { get; set; }
        public Guid TariffId { get; set; }

        public Person Person { get; set; }
        public Tariff Tariff { get; set; }

    }
}
