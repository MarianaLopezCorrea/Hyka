
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Hyka.Models;

namespace Hyka.Dtos
{
    public class PersonDto
    {
        [Required]
        public String Id { get; set; }

        [Required]
        public String DocumentType { get; set; }

        [Required]
        public String FullName { get; set; }

        [Required]
        public String Gender { get; set; }

        [Required]
        public String Country { get; set; }

        public String DepartmentOfBirth { get; set; }

        public String MunicipalityOfBirth { get; set; }

        public String District { get; set; }

        public String BloodType { get; set; }

        [JsonIgnore]
        public DateTime RegisterDateTime { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.UtcNow.DateTime, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        [JsonIgnore]
        [ForeignKey("TariffId")]
        public String TariffId { get; set; }

        public String CardId { get; set; }

        [Required]
        public int Age { get; set; }

        [JsonIgnore]
        public bool HasPet { get; set; }

        [JsonIgnore]
        public bool IsLocal { get; set; }

        public static Person Map(PersonDto personDto)
        {
            Person person = new Person();
            
            person.Age = personDto.Age;
            person.BloodType = personDto.BloodType;
            personDto.CardId = personDto.CardId;
            person.Country = personDto.Country;
            person.DepartmentOfBirth = personDto.DepartmentOfBirth;
            person.District = personDto.District;
            person.DocumentType = personDto.DocumentType;
            person.FullName = personDto.FullName;
            person.Gender = personDto.Gender;
            person.Id = personDto.Id;
            person.IsLocal = personDto.IsLocal;
            person.MunicipalityOfBirth = personDto.MunicipalityOfBirth;
            person.TariffId = personDto.TariffId;
            return person;
        }
        public static PersonDto Map(Person person)
        {
            PersonDto personDto = new PersonDto();
            personDto.Age = person.Age;
            personDto.BloodType = person.BloodType;
            personDto.CardId = person.CardId;
            personDto.Country = person.Country;
            personDto.DepartmentOfBirth = person.DepartmentOfBirth;
            personDto.District = person.District;
            personDto.DocumentType = person.DocumentType;
            personDto.FullName = person.FullName;
            personDto.Gender = person.Gender;
            personDto.Id = person.Id;
            personDto.IsLocal = person.IsLocal;
            personDto.MunicipalityOfBirth = person.MunicipalityOfBirth;
            personDto.TariffId = person.TariffId;
            return personDto;
        }
    }
}