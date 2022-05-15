using Hyka.Data;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Hyka.Areas.Identity.PoliciesDefinition;

namespace Hyka.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_BLOCKBUSTER)]
    public class DecoderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private static List<KeyValuePair<Person, Tariff>> _order = new();
        private static int _total;

        public DecoderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Decode()
        {
            ViewBag.Total = _total;
            ViewBag.Order = _order;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Decode(Barcode barcode)
        {
            if (ModelState.IsValid)
            {
                String _code = barcode.Code;
                Match zeroMatch = Regex.Match(_code, @"(M|F)([0-9]{8})(0|)([0-9]{5})", RegexOptions.Multiline);
                Match rhMatch = Regex.Match(_code, @"(A|B|O|AB)(\+|-)", RegexOptions.Multiline);
                if (rhMatch.Success && zeroMatch.Success)
                {
                    Person person = new();
                    String DaneId;
                    // Clean string Barcode
                    _code = _code.Substring(0, rhMatch.Index);
                    // Index to save the end of the name
                    int i = 0;
                    for (i = _code.Length - 17; i > 0; i--)
                    {
                        if (Char.IsDigit(_code[i])) break;

                    }
                    person.Id = _code.Substring(i - 9, 10).TrimStart('0');
                    person.DocumentType = _code.ElementAt(0).Equals('I') ? "IT" : "CC";
                    person.BloodType = rhMatch.Value;
                    // Zero match onnly has 4 groups
                    person.Gender = zeroMatch.Groups[1].Value;
                    person.Age = DateTime.UtcNow.Year - Int32.Parse(zeroMatch.Groups[2].Value.Substring(0, 4));
                    // Group[3] must be 0 or null
                    DaneId = zeroMatch.Groups[4].Value;
                    // Clean full name
                    String fullName = _code.Substring(i + 1, _code.Length - 17 - i);
                    person.FullName = Regex.Replace(fullName, @"([^A-ZÑ])+", " ").TrimEnd();

                    person.Country = "COLOMBIA";
                    Territory territory = getTerritory(DaneId);
                    if (territory != null)
                    {
                        person.Department = territory.DepartmentName;
                        person.Municipality = territory.MunicipalityName;
                    }
                    else
                    {
                        TempData["error"] = "Territoy not found.";
                        return RedirectToAction("Decode");
                    }
                    verifyOrCreateTariff();
                    Tariff tariff = getTariff(person);
                    person.TariffId = tariff.Id;
                    KeyValuePair<Person, Tariff> information = new(person, tariff);
                    _order.Add(information);
                    _total += tariff.Price;
                    TempData["success"] = "User added correctly";
                    return RedirectToAction("Decode");
                }
            }
            TempData["error"] = "Invalid Barcode";
            return RedirectToAction("Decode");
        }

        public async Task<IActionResult> FinishOrder()
        {
            foreach (var pair in _order)
            {
                Person person = pair.Key;
                Tariff tariff = pair.Value;
                History history = new(Guid.NewGuid(), person.Id, tariff.Name, tariff.Price);
                if (await _db.People.FindAsync(person.Id) == null)
                {
                    await _db.People.AddAsync(person);
                }
                await _db.Histories.AddAsync(history);

            }
            await _db.SaveChangesAsync();
            _order.Clear();
            _total = 0;
            TempData["success"] = "Order complete successful";
            return RedirectToAction("Decode");
        }

        public IActionResult Delete(string id)
        {
            var pair = _order.Find(pair => pair.Key.Id == id);
            _order.Remove(pair);
            _total -= pair.Value.Price;
            return RedirectToAction("Decode");
        }

        private async void verifyOrCreateTariff()
        {
            if (_db.Tariffs.Any())
                return;

            List<Tariff> tariff = new(){
                new Tariff("1C", "Exento", 0),
                new Tariff("2C", "Local", 1500),
                new Tariff("3C", "Infante", 6500),
                new Tariff("4C", "Nacional", 10500),
                new Tariff("5C", "Extranjero", 40500)
            };
            await _db.Tariffs.AddRangeAsync(tariff);
            await _db.SaveChangesAsync();
        }

        private Tariff getTariff(Person person)
        {
            Tariff tariff = null;
            if (person.Age < 5 || person.Age > 59)
            {
                tariff = _db.Tariffs
                    .Where(t => t.Name.Equals("Exento"))
                    .FirstOrDefault();
            }
            else if (person.Municipality.Equals("FACATATIVA"))
            {
                tariff = _db.Tariffs
                    .Where(t => t.Name.Equals("Local"))
                    .FirstOrDefault();
            }
            else if (person.Age > 5 && person.Age < 13)
            {
                tariff = _db.Tariffs
                    .Where(t => t.Name.Equals("Infante"))
                    .FirstOrDefault();
            }
            else
            {
                tariff = _db.Tariffs
                    .Where(t => t.Name.Equals("Nacional"))
                    .FirstOrDefault();
            }

            return tariff != null ? tariff : _db.Tariffs.Where(t => t.Name.Equals("Nacional")).FirstOrDefault();
        }

        private Territory getTerritory(String DaneId)
        {
            Territory territory = _db.Territories
                                    .Where(t => t.DaneId == DaneId)
                                    .FirstOrDefault();
            return territory;
        }

    }
}
