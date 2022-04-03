using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Controllers
{
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

        // GET
        public IActionResult Create()
        {
            return View();
        }
        // POST
        [HttpPost]
        // Prevent request forgery 
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Tariff obj)
        {
            if (obj.TariffName == obj.Price.ToString())
            {
                ModelState.AddModelError("Name", "Name can't be equal to Display Order ");
            }

            if (ModelState.IsValid)
            {
                _db.Tariff.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Tariff Created Correctly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET
        public IActionResult Edit(Guid id)
        {
            var TariffFromDb = _db.Tariff.Find(id);
            //var BlockbusterFromDbFirst = _db.Blockbusters.FirstOrDefault(c => c.Id == id);
            //var BlockbusterFromDbSingle = _db.Blockbusters.SingleOrDefault(c => c.Id == id);
            return TariffFromDb == null ?
                NotFound() : View(TariffFromDb);
        }
        // POST 
        [HttpPost]
        // Prevent request falsification wi
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Tariff obj)
        {
            if (obj.TariffName == obj.Price.ToString())
            {
                ModelState.AddModelError("Name", "Name can't be equal to Display Order ");
            }

            if (ModelState.IsValid)
            {
                _db.Tariff.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Tariff Updated Correctly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var TariffFromDb = _db.Tariff.Find(id);
            //var BlockbusterFromDbFirst = _db.Blockbusters.FirstOrDefault(c => c.Id == id);
            //var BlockbusterFromDbSingle = _db.Blockbusters.SingleOrDefault(c => c.Id == id);
            return TariffFromDb == null ?
                NotFound() : View(TariffFromDb);
        }
        // POST 
        [HttpPost, ActionName("Delete")]
        // Prevent request falsification wi
        [AutoValidateAntiforgeryToken]
        public IActionResult DeletePOST(Guid? id)
        {
            var obj = _db.Tariff.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Tariff.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Tariff Deleted Correctly";
            return RedirectToAction("Index");
        }

    }
}

