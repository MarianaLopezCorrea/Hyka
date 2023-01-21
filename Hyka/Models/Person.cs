using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hyka.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonPropertyName("Cédula")]
        public String Id { get; set; }

        [JsonPropertyName("Tipo")]
        [JsonPropertyOrder(-1)]
        public String DocumentType { get; set; }

        [JsonPropertyName("Nombre")]
        public String FullName { get; set; }

        [JsonPropertyName("Género")]
        public String Gender { get; set; }

        [JsonPropertyName("País")]
        public String Country { get; set; }

        [JsonPropertyName("Departamento de Nacimiento")]
        public String DepartmentOfBirth { get; set; }

        [JsonPropertyName("Municipio de Nacimiento")]
        public String MunicipalityOfBirth { get; set; }

        [JsonPropertyName("Barrio")]
        public String District { get; set; }

        [JsonPropertyName("RH")]
        public String BloodType { get; set; }

        [JsonPropertyName("Tarje Joven")]
        public String CardId { get; set; }

        [JsonPropertyName("Edad")]
        public int Age { get; set; }

        [JsonPropertyName("Es AmigoPAF")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("Fecha de registro")]
        [JsonPropertyOrder(1)]
        public DateTime RegisterDateTime { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.UtcNow.DateTime, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        [JsonPropertyName("Tarifa Asignada")]
        [ForeignKey("TariffId")]
        public String TariffId { get; set; }

    }

}