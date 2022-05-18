using Hyka.Data;
using Hyka.Models;

namespace Hyka.Services
{
    public class TariffService : ITariffService
    {
        private readonly ApplicationDbContext _db;

        public TariffService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Any()
        {
            return _db.Tariffs.Any();
        }

        public int CreateDefaultTariffs()
        {
            List<Tariff> tariffs = new(){
                new Tariff("1C", "Exento", 0),
                new Tariff("2C", "Local", 1500),
                new Tariff("3C", "Infante", 6500),
                new Tariff("4C", "Nacional", 10500),
                new Tariff("5C", "Extranjero", 40500)
            };
            _db.AddRange(tariffs);
            return _db.SaveChanges();
        }

        public IEnumerable<Tariff> Get()
        {
            return _db.Tariffs.AsEnumerable();
        }

        public Tariff GetById(string id)
        {
            return _db.Tariffs.Find(id);
        }

        public Tariff GetByPerson(Person person)
        {
            // If don't have category, 'Extranjero' is default
            String tariffId = "5C";
            if (person.Age < 5 || person.Age > 59)
            {
                tariffId = "1C";
            }
            else if (person.Municipality == "FACATATIVA")
            {
                tariffId = "2C";
            }
            else if (person.Age > 5 && person.Age < 13)
            {
                tariffId = "3C";
            }
            else if (person.Country == "COLOMBIA")
            {
                tariffId = "4C";
            }
            return _db.Tariffs.Find(tariffId);
        }
    }
}