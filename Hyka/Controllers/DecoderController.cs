using Hyka.Data;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Hyka.Areas.Identity.PoliciesDefinition;

namespace Hyka.Controllers
{
    public class DecoderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DecoderController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Authorize(Policy = Policy.REQUIRE_BLOCKBUSTER)]
        public IActionResult Decode()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = Policy.REQUIRE_BLOCKBUSTER)]
        public IActionResult Decode(Barcode barcode)
        {
            if (ModelState.IsValid)
            {
                String _code = barcode.Code;
                Match zeroMatch = Regex.Match(_code, @"(M|F)([0-9]{8})(0|)([0-9]{5})", RegexOptions.Multiline);
                Match rhMatch = Regex.Match(_code, @"(A|B|O|AB)(\+|-)", RegexOptions.Multiline);
                if (rhMatch.Success && zeroMatch.Success)
                {
                    Person person = new Person();
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

                    if (_db.Users.Find(person.Id) != null)
                    {
                        TempData["error"] = "User already exists";
                        return RedirectToAction("Decode");
                    }

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

                    Territory territory = setTerritory(DaneId);
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
                    person.TariffId = setTariff(person);
                    _db.Add(person);
                    _db.SaveChanges();
                    TempData["success"] = "User created correctly";
                    return RedirectToAction("Decode");
                }
            }
            TempData["error"] = "Invalid Barcode";
            return RedirectToAction("Decode");
        }

        private void verifyOrCreateTariff()
        {
            if (_db.Tariff.Any())
                return;

            List<Tariff> tariff = new(){
                new Tariff("1C", "Facatativeño", 1500),
                new Tariff("2C", "Colombiano", 10500),
                new Tariff("3C", "Extranjero", 40500),
                new Tariff("4C", "Exento", 0)
            };
            _db.Tariff.AddRange(tariff);
            _db.SaveChanges();

        }
        private string setTariff(Person person)
        {
            Tariff tariff = null;
            if (person.Age < 8)
            {
                tariff = _db.Tariff
                .Where(t => t.Name.Equals("Exento"))
                .FirstOrDefault();
            }
            else if (person.Municipality.Equals("FACATATIVA"))
            {
                tariff = _db.Tariff
                    .Where(t => t.Name.Equals("Facatativeño"))
                    .FirstOrDefault();
            }
            else if (!person.Municipality.Equals("FACATATIVA"))
            {
                tariff = _db.Tariff
                    .Where(t => t.Name.Equals("Colombiano"))
                    .FirstOrDefault();
            }

            return tariff != null ? tariff.Id : "3C";
        }

        private Territory setTerritory(String DaneId)
        {
            Territory territory = _db.Territories
                                    .Where(t => t.DaneId == DaneId)
                                    .FirstOrDefault();
            return territory;
        }

    }
}