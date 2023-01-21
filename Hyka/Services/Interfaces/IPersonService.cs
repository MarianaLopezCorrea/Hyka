using Hyka.Dtos;
using Hyka.Models;

namespace Hyka.Services
{
    public interface IPersonService
    {
        public IQueryable<Person> GetAll();

        public IEnumerable<Person> GetBetween(DateTime start, DateTime end);

        public Person GetById(String id);

        public int Create(Person person);

        public int Update(Person person);

        public PersonDto FromBarcode(String barcode);
    }
}
