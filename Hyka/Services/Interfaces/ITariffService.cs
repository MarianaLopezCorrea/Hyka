using Hyka.Models;

namespace Hyka.Services
{
    public interface ITariffService
    {

        public IEnumerable<Tariff> Get();

        public bool Create(Tariff tariff);

        public bool Update(Tariff tariff);

        public bool Delete(Guid id);

        public Tariff GetById(Guid id);

        public Tariff GetByPerson(Person person);

        public int CreateDefaultTariffs();

        public bool Any();

    }
}
