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
                Regex regexRh = new Regex(@"(A|B|O|AB)(\+|-)");
                Match rhmatch = regexRh.Match(barcode.Code);
                Person person = new Person();
                if (rhmatch.Success)
                {
                    String _code = barcode.Code;
                    person.DocumentType = _code.ElementAt(0).Equals('I') ? "IT" : "CC";
                    person.BloodType = rhmatch.Value;
                    _code = _code.Substring(0, rhmatch.Index);
                    string DaneId = _code.Substring(_code.Length - 6, 5);
                    person.Age = DateTime.UtcNow.Year - Int32.Parse(_code.Substring(_code.Length - 14, 4));
                    person.Gender = _code.Substring(_code.Length - 15, 1);
                    int i = 0;
                    for (i = _code.Length - 17; i > 0; i--)
                    {
                        if (Char.IsDigit(_code[i]))
                        {
                            break;
                        }
                    }
                    String fullName = _code.Substring(i + 1, _code.Length - 17 - i);
                    person.FullName = Regex.Replace(fullName, @"([^A-ZÑ])+", " ").TrimEnd();
                    person.Id = _code.Substring(i - 9, 10);

                    if (_db.Users.Find(person.Id) != null)
                    {
                        TempData["error"] = "User already exists";
                        return RedirectToAction("Decode");
                    }
                    var territory = _db.Territories
                        .Where(t => t.DaneId == DaneId)
                        .FirstOrDefault();

                    if (territory != null)
                    {
                        person.Department = territory.DepartmentName;
                        person.Municipality = territory.MunicipalityName;
                    }
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