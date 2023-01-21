using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hyka.Models
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonIgnore]
        public String Id { get; set; }

        [Required]
        [JsonPropertyName("Fecha de visita")]
        [JsonPropertyOrder(1)]
        public DateTime VisitDateTime { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.UtcNow.DateTime, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        [Required]
        [ForeignKey("PersonId")]
        [JsonPropertyName("Cédula")]
        public String PersonId { get; set; }

        [Required]
        [ForeignKey("BlockbusterId")]
        [JsonPropertyName("Registrado Por")]
        public String RegisteredBy { get; set; }

        [Required]
        [JsonPropertyName("Tarifa")]
        public String TariffName { get; set; }

        [Required]
        [JsonPropertyName("Valor Pagado")]
        public int Price { get; set; }

        [Required]
        [JsonPropertyName("Entró Mascota")]
        public bool HasPet { get; set; }

        public Record() { }

        public Record(String id, string personId, string registeredBy, string tariffName, int price, bool hasPet)
        {
            Id = id;
            PersonId = personId;
            RegisteredBy = registeredBy;
            TariffName = tariffName;
            Price = price;
            HasPet = hasPet;
        }
    }
}
