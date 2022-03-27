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

        public IActionResult Index()
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
                Decoder decoder = new Decoder();
                decoder.Code = barcode.Code;
                Person person = new Person();
                person = decoder.Decode();
                _db.Add(person);
                _db.SaveChanges();
                TempData["success"] = "User created correctly";
                return RedirectToAction("Index");
            }
            return View(barcode);
        }



    }
}