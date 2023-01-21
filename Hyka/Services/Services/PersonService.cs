using System.Text.RegularExpressions;
using Hyka.Data;
using Hyka.Dtos;
using Hyka.Models;
using Microsoft.EntityFrameworkCore;

namespace Hyka.Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITariffService _tariffService;
        private readonly ITerritoryService _territoryService;
        private readonly ILogger<Person> _logger;

        public PersonService(
            ApplicationDbContext db,
            ITariffService tariffService,
            ITerritoryService territoryService,
             ILogger<Person> logger)
        {
            _db = db;
            _tariffService = tariffService;
            _territoryService = territoryService;
            _logger = logger;
        }

        public int Create(Person person)
        {
            _db.People.Add(person);
            return _db.SaveChanges();
        }

        public int Update(Person person)
        {
            // Detach entity from context 
            _db.Entry(person).State = EntityState.Detached;
            _db.People.Update(person);
            return _db.SaveChanges();
        }

        public IQueryable<Person> GetAll()
        {
            return _db.People.AsQueryable();
        }

        public IEnumerable<Person> GetBetween(DateTime start, DateTime end)
        {
            return _db.People.Where(p =>
                p.RegisterDateTime.Date >= start.Date && p.RegisterDateTime.Date <= end.Date
            );
        }

        public Person GetById(String id)
        {
            return _db.People.Find(id);
        }

        public PersonDto FromBarcode(String barcode)
        {
            try
            {
                Match documentNumber = Regex.Match(barcode, @"^[0-9]+$");
                if (documentNumber.Success)
                    return PersonDto.Map(GetById(barcode));
                Match zeroMatch = Regex.Match(barcode, @"(M|F)([0-9]{8})(0|)([0-9]{5})", RegexOptions.Multiline);
                Match rhMatch = Regex.Match(barcode, @"(A|B|O|AB)(\+|-)", RegexOptions.Multiline);
                if (rhMatch.Success && zeroMatch.Success)
                {
                    var personDto = new PersonDto();
                    String daneId;
                    // Clean string Barcode
                    barcode = barcode.Substring(0, rhMatch.Index);
                    // Index to save the end of the name    
                    int i = 0;
                    for (i = barcode.Length - 17; i > 0; i--)
                    {
                        if (Char.IsDigit(barcode[i])) break;
                    }
                    personDto.Id = barcode.Substring(i - 9, 10).TrimStart('0');
                    var person = GetById(personDto.Id);
                    if (person != null)
                        return PersonDto.Map(person);
                    personDto.DocumentType = barcode.ElementAt(0).Equals('I') ? "TI" : "CC";
                    personDto.BloodType = rhMatch.Value;
                    // Zero match only has 4 groups
                    personDto.Gender = zeroMatch.Groups[1].Value;
                    personDto.Age = DateTime.UtcNow.Year - Int32.Parse(zeroMatch.Groups[2].Value.Substring(0, 4));
                    // Group[3] must be 0 or null
                    daneId = zeroMatch.Groups[4].Value;
                    // Clean full name
                    String fullName = barcode.Substring(i + 1, barcode.Length - 17 - i);
                    personDto.FullName = Regex.Replace(fullName, @"([^A-ZÑ])+", " ").TrimEnd();
                    var territory = _territoryService.GetById(daneId);
                    if (territory != null)
                    {
                        personDto.Country = "COLOMBIA";
                        personDto.DepartmentOfBirth = territory.DepartmentName;
                        personDto.MunicipalityOfBirth = territory.MunicipalityName;
                        personDto.IsLocal = personDto.MunicipalityOfBirth == "FACATATIVA" ? true : false;
                    }
                    return personDto;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
            return null;
        }

    }
}