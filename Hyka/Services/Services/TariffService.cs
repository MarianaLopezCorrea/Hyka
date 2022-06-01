using Hyka.Data;
using Hyka.Models;
using Microsoft.EntityFrameworkCore;

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

        public bool Create(Tariff tariff)
        {
            var result = _db.Tariffs.Find(tariff.Id);
            if (result != null) return false;
            _db.Add(tariff);
            if (_db.SaveChanges() == 1) return true;
            return false;
        }

        public int CreateDefaultTariffs()
        {

            // List<Tariff> tariffs = new(){

            //     new Tariff("1C", "Exento", 0, new Rule(){
            //         Attribute = "Age",
            //         Condition = "or",
            //         Value = "<5 or >59"
            //     }),
            // String str = "(<|>)([0-9]{1,2})\s+(or)\s+(<|>)([0-9]{1,2})";
            //     new Tariff("2C", "Local", 1500),
            //     new Tariff("3C", "Infante", 6500),
            //     new Tariff("4C", "Nacional", 10500),
            //     new Tariff("5C", "Extranjero", 40500)
            // };
            // _db.AddRange(tariffs);
            return _db.SaveChanges();
        }

        public bool Delete(Guid id)
        {
            var tariff = _db.Tariffs.Find(id);
            if (tariff == null) return false;
            _db.Tariffs.Remove(tariff);
            if (_db.SaveChanges() == 1) return true;
            return false;
        }

        public IEnumerable<Tariff> Get()
        {
            return _db.Tariffs.AsEnumerable();
        }

        public Tariff GetById(Guid id)
        {
            var tariff = _db.Tariffs.Find(id);
            tariff.Rule = _db.Rules.Find(tariff.RuleId);
            return tariff;
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

        public bool Update(Tariff tariff)
        {
            _db.Tariffs.Update(tariff);
            if (_db.SaveChanges() == 1) return true;
            return false;
        }
    }
}