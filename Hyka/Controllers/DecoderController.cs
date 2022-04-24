using Hyka.Data;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Hyka.Controllers
{
    public class DecoderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DecoderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Decode()
        {
            return View();
        }

        [HttpPost]
        // Prevent request forgery 
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

                    var territory = _db.Territories
                        .Where(t => t.DaneId == DaneId)
                        .FirstOrDefault();

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

                    Category category = new Category();
                        
                    if (person.Age < 8)
                    {

                        var tari = _db.Tariff
                        .Where(t => t.TariffName == "eximido")
                        .FirstOrDefault();


                        category.TariffId = tari.Id;
                        category.PersonId = person.Id;

                    }
                    else if (person.Municipality == "FACATATIVA")
                    {

                        var tari = _db.Tariff
                            .Where(t => t.TariffName == "local")
                            .FirstOrDefault();


                        category.TariffId = tari.Id;
                        category.PersonId = person.Id;

                    }
                    else
                    {

                        var tari = _db.Tariff
                            .Where(t => t.TariffName == "nacional")
                            .FirstOrDefault();


                        category.TariffId = tari.Id;
                        category.PersonId = person.Id;
                    }


                    _db.Category.Add(category);
                    _db.Add(person);
                    _db.SaveChanges();
                    TempData["success"] = "User created correctly";
                    return RedirectToAction("Decode");
                }
            }
            TempData["error"] = "Invalid Barcode";
            return RedirectToAction("Decode");
        }

    }
}