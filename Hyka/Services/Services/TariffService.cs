using Hyka.Data;
using Hyka.Dtos;
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

        public bool Update(Tariff tariff)
        {
            _db.Entry(tariff).State = EntityState.Detached;
            _db.Update(tariff);
            return _db.SaveChanges() > 0;
        }

        public IEnumerable<Tariff> Get()
        {
            return _db.Tariffs.AsEnumerable();
        }

        public Tariff GetById(string id)
        {
            return _db.Tariffs.Find(id);
        }

        public Tariff GetByPerson(PersonDto personDto)
        {
            var tariffId = string.Empty;
            switch (personDto)
            {
                case PersonDto p when p.Age < 5 || p.Age > 62:
                    tariffId = "EXENTO";
                    break;
                case PersonDto p when p.CardId != null:
                    tariffId = "TARJETAJOVEN";
                    break;
                case PersonDto p when p.MunicipalityOfBirth == "FACATATIVA" || p.IsLocal:
                    tariffId = "AMIGOPAF";
                    break;
                case PersonDto p when p.Age > 5 && p.Age < 13:
                    tariffId = "INFANTE";
                    break;
                case PersonDto p when p.Country == "COLOMBIA":
                    tariffId = "NACIONAL";
                    break;
                default:
                    tariffId = "EXTRANJERO";
                    break;
            }
            return _db.Tariffs.Find(tariffId);
        }

    }
}