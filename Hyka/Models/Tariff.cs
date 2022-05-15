using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Tariff
    {
        [Key]
        public String Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public int Price { get; set; }

        public Tariff(string id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public Tariff()
        {
        }
    }
}
