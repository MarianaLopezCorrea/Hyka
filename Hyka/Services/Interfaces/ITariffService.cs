using Hyka.Dtos;
using Hyka.Models;

namespace Hyka.Services
{
    public interface ITariffService
    {
        public IEnumerable<Tariff> Get();

        public Tariff GetById(String id);

        public Tariff GetByPerson(PersonDto personDto);

        public bool Update(Tariff tariff);

        public bool Any();
    }
}
