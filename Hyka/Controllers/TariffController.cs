using Hyka.Areas.Identity.PoliciesDefinition;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;
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
        // [Authorize(Policy = Policy.REQUIRE_ADMIN)]
        [Authorize(Roles = $"{Roles.ADMIN}")]
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
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create(Tariff tariff)
        {
            var result = await _db.Tariff.FindAsync(tariff.Id);
            if (result != null)
            {
                ModelState.AddModelError(string.Empty, "Tariff already exist");
                return View();
            }

            if (ModelState.IsValid)
            {

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

