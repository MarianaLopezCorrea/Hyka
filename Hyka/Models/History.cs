using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class History
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime VisitDateTime { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("PersonId")]
        public String PersonId { get; set; }

        [Required]
        public String TariffName { get; set; }

        [Required]
        public int Price { get; set; }

        public History(Guid Id, String PersonId, String TariffName, int Price)
        {
            this.Id = Id;
            this.PersonId = PersonId;
            this.TariffName = TariffName;
            this.Price = Price;
        }

        public History() { }

    }
}
