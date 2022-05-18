using Hyka.Models;

namespace Hyka.Services
{
    public interface IPersonService
    {
        public IEnumerable<Person> Get();

        public Person GetById(String id);

        public int Create(Person person);

        public Person FromBarcode(Barcode barcode);

    }
}
