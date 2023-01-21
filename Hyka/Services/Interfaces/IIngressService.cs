using Hyka.Dtos;
using Hyka.Models;

namespace Hyka.Services
{
    public interface IIngressService
    {
        public bool AddToIngressList(ISession session, PersonDto personDto);

        public bool RemovePersonFromIngressList(ISession session, String id);

        public void SaveIngressList(HttpContext context);

        public bool HasPet(ISession session, String id);

        public PersonDto Decode(String barcode);
        
    }
}
