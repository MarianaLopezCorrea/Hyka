using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Controllers
{
    [Authorize]
    public class TariffController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TariffController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Tariff> tariffList = _db.Tariff;
            return View(tariffList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Tariff tariff)
        {

            if (ModelState.IsValid)
            {
                if (_db.Tariff.Find(tariff.Id) != null)
                {
                    TempData["error"] = "Tariff already exists";
                    return RedirectToAction("Index");
                }
                _db.Tariff.Add(tariff);
                _db.SaveChanges();
                TempData["success"] = "Tariff Created Correctly";
                return RedirectToAction("Index");
            }
            return View(tariff);
        }

        public IActionResult Edit(String id)
        {
            var TariffFromDb = _db.Tariff.Find(id);
            return TariffFromDb == null ?
                NotFound() : View(TariffFromDb);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                _db.Tariff.Update(tariff);
                _db.SaveChanges();
                TempData["success"] = "Tariff Updated Correctly";
                return RedirectToAction("Index");
            }
            return View(tariff);
        }



    }
}

