using Hyka.Models;

namespace Hyka.Services
{
    public interface ITariffService
    {

        public IEnumerable<Tariff> Get();

        public Tariff GetById(String id);

        public Tariff GetByPerson(Person person);

        public int CreateDefaultTariffs();

        public bool Any();

    }
}
