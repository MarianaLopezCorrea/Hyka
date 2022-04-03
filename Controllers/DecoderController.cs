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
                    person.DocumentType = barcode.Code.ElementAt(0).Equals('I') ? "IT" : "CC";
                    person.BloodType = rhmatch.Value;
                    barcode.Code = barcode.Code.Substring(0, rhmatch.Index);
                    string DaneId = barcode.Code.Substring(barcode.Code.Length - 6, 5);
                    person.Age = DateTime.UtcNow.Year - Int32.Parse(barcode.Code.Substring(barcode.Code.Length - 14, 4));
                    person.Gender = barcode.Code.Substring(barcode.Code.Length - 15, 1);
                    int i = 0;
                    for (i = barcode.Code.Length - 17; i > 0; i--)
                    {
                        if (Char.IsDigit(barcode.Code[i]))
                        {
                            break;
                        }
                    }
                    String fullName = barcode.Code.Substring(i + 1, barcode.Code.Length - 17 - i);
                    person.FullName = Regex.Replace(fullName, @"([^A-ZÑ])+", " ").TrimEnd();
                    person.Id = barcode.Code.Substring(i - 9, 10);
                    var territory = _db.Territories
                    .Where(t => t.DaneId == DaneId)
                     .FirstOrDefault();
                    if (territory != null)
                    {
                        person.Department = territory.DepartmentName;
                        person.Municipality = territory.MunicipalityName;
                    }
                    
                    if (_db.find(person.id)){
                        //TODO
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