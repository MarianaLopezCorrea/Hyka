using Hyka.Data;
using Hyka.Models;
using Hyka.Extensions;
using Hyka.Dtos;

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
        public PersonDto Decode(String barcode)
        {
            return _personService.FromBarcode(barcode);
        }

        public void SaveIngressList(HttpContext context)
        {
            var blockbusterName = context.User.Identity.Name;
            var ingressList = context.Session.GetObject<List<KeyValuePair<PersonDto, Tariff>>>("INGRESS_LIST");
            var recordId = Guid.NewGuid().ToString();
            int ingressCount = 0;
            foreach (var pair in ingressList)
            {
                var personDto = pair.Key;
                var tariff = pair.Value;
                var record = new Record(
                    recordId + "-" + ingressCount++.ToString(),
                    personDto.Id,
                    blockbusterName,
                    tariff.Name,
                    tariff.Price,
                    personDto.HasPet
                );
                if (_personService.GetById(personDto.Id) == null)
                    _personService.Create(PersonDto.Map(personDto));
                else
                    _personService.Update(PersonDto.Map(personDto));

                _historyService.Create(record);
            }
            context.Session.Clear();
        }

        public bool AddToIngressList(ISession session, PersonDto personDto)
        {
            var ingressList = session.GetObject<List<KeyValuePair<PersonDto, Tariff>>>("INGRESS_LIST");
            if (ingressList.Exists(p => p.Key.Id == personDto.Id))
                ingressList.RemoveAll(p => p.Key.Id == personDto.Id);
            if (personDto.District != null)
                personDto.IsLocal = true;
            var tariff = _tariffService.GetByPerson(personDto);
            if (tariff != null)
                personDto.TariffId = tariff.Id;
            ingressList.Add(new(personDto, tariff));
            var total = ingressList.Sum(t => t.Value.Price);
            session.SetObject("INGRESS_LIST", ingressList);
            session.SetObject("TOTAL", total);
            return true;
        }

        public bool RemovePersonFromIngressList(ISession session, string id)
        {
            var ingressList = session.GetObject<List<KeyValuePair<PersonDto, Tariff>>>("INGRESS_LIST");
            var count = ingressList.RemoveAll(p => p.Key.Id == id);
            var total = ingressList.Sum(t => t.Value.Price);
            session.SetObject("INGRESS_LIST", ingressList);
            session.SetObject("TOTAL", total);
            return count > 0;
        }

        public bool HasPet(ISession session, string id)
        {
            var ingressList = session.GetObject<List<KeyValuePair<PersonDto, Tariff>>>("INGRESS_LIST");
            var personDto = ingressList.Find(p => p.Key.Id == id).Key;
            if (personDto == null)
                return false;
            personDto.HasPet = !personDto.HasPet;
            session.SetObject("INGRESS_LIST", ingressList);
            return true;
        }
    }
}