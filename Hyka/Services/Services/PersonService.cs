using System.Text.RegularExpressions;
using Hyka.Data;
using Hyka.Models;

namespace Hyka.Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITariffService _tariffService;
        private readonly ITerritoryService _territoryService;

        public PersonService(
            ApplicationDbContext db,
            ITariffService tariffService,
            ITerritoryService territoryService
        )
        {
            _db = db;
            _tariffService = tariffService;
            _territoryService = territoryService;
        }

        public int Create(Person person)
        {
            _db.People.Add(person);
            return _db.SaveChanges();
        }

        public IEnumerable<Person> Get()
        {
            return _db.People.AsEnumerable();
        }

        public Person GetById(String id)
        {
            return _db.People.Find(id);
        }

        public Person FromBarcode(Barcode barcode)
        {
            String _code = barcode.Code;
            Match zeroMatch = Regex.Match(_code, @"(M|F)([0-9]{8})(0|)([0-9]{5})", RegexOptions.Multiline);
            Match rhMatch = Regex.Match(_code, @"(A|B|O|AB)(\+|-)", RegexOptions.Multiline);
            if (rhMatch.Success && zeroMatch.Success)
            {
                Person person = new();
                String daneId;
                // Clean string Barcode
                _code = _code.Substring(0, rhMatch.Index);
                // Index to save the end of the name    
                int i = 0;
                for (i = _code.Length - 17; i > 0; i--)
                {
                    if (Char.IsDigit(_code[i])) break;
                }
                person.Id = _code.Substring(i - 9, 10).TrimStart('0');
                person.DocumentType = _code.ElementAt(0).Equals('I') ? "IT" : "CC";
                person.BloodType = rhMatch.Value;
                // Zero match only has 4 groups
                person.Gender = zeroMatch.Groups[1].Value;
                person.Age = DateTime.UtcNow.Year - Int32.Parse(zeroMatch.Groups[2].Value.Substring(0, 4));
                // Group[3] must be 0 or null
                daneId = zeroMatch.Groups[4].Value;
                // Clean full name
                String fullName = _code.Substring(i + 1, _code.Length - 17 - i);
                person.FullName = Regex.Replace(fullName, @"([^A-ZÑ])+", " ").TrimEnd();
                person.Country = "COLOMBIA";
                var territory = _territoryService.GetById(daneId);
                if (territory != null)
                {
                    person.Department = territory.DepartmentName;
                    person.Municipality = territory.MunicipalityName;
                }
                if (!_tariffService.Any())
                    _tariffService.CreateDefaultTariffs();
                var tariff = _tariffService.GetByPerson(person);
                if (tariff != null)
                    person.TariffId = tariff.Id.ToString();
                return person;
            }
            return null;
        }

    }
}