using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Identity;

namespace Hyka.Services
{
    public class IngressService : IIngressService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPersonService _personService;
        private readonly ITariffService _tariffService;
        private readonly ITerritoryService _territoryService;
        private readonly IHistoryService _historyService;


        public IngressService(
            ApplicationDbContext db,
            IPersonService personService,
            ITariffService tariffService,
            ITerritoryService territoryService,
            IHistoryService historyService)
        {
            _db = db;
            _personService = personService;
            _tariffService = tariffService;
            _territoryService = territoryService;
            _historyService = historyService;
        }

        public bool AddToGroup(Person person)
        {
            if (Group.PeopleList.Exists(p => p.Key.Id == person.Id))
                return false;
            var tariff = _tariffService.GetByPerson(person);
            if (person.TariffId == null)
                person.TariffId = tariff.Id;
            KeyValuePair<Person, Tariff> personInfo = new(person, tariff);
            Group.PeopleList.Add(personInfo);
            Group.Total = Group.PeopleList.Sum(t => t.Value.Price);
            return true;
        }

        public Person Decode(Barcode barcode)
        {
            return _personService.FromBarcode(barcode);
        }

        public int RemovePersonFromGroup(string id)
        {
            var number = Group.PeopleList.RemoveAll(p => p.Key.Id == id);
            Group.Total = Group.PeopleList.Sum(t => t.Value.Price);
            return number;
        }

        public void SaveGroup()
        {
            foreach (var pair in Group.PeopleList)
            {
                var person = pair.Key;
                var tariff = pair.Value;
                var history = new History(Guid.NewGuid(), person.Id, tariff.Name, tariff.Price);
                if (_personService.GetById(person.Id) == null)
                    _personService.Create(person);
                _historyService.Create(history);
            }
            Group.PeopleList.Clear();
            Group.Total = 0;
        }
    }
}