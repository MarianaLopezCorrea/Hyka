using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Required]
        [JsonIgnore]
        public DateTime UpdateTime { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.UtcNow.DateTime, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        public Tariff(string id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public Tariff() { }
    }
}
