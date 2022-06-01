using Hyka.Models;

namespace Hyka.Services
{
    public interface IIngressService
    {
        public Person Decode(Barcode barcode);

        public bool AddToGroup(Person person);

        public void SaveGroup();

        public int RemovePersonFromGroup(String id);

        public bool SetAsLocal(String id);

    }
}
