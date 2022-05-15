using Hyka.Areas.Identity.PoliciesDefinition;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Data;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_ADMIN)]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public class TariffController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TariffController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var tariffList = _db.Tariffs;
            return View(tariffList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Tariff tariff)
        {
            var result = await _db.Tariffs.FindAsync(tariff.Id);
            if (result != null)
            {
                ModelState.AddModelError(string.Empty, "Tariff already exist");
                return View();
            }

            if (ModelState.IsValid)
            {
                _db.Tariffs.Add(tariff);
                _db.SaveChanges();
                TempData["success"] = "Tariff Created Correctly";
                return RedirectToAction("Index");
            }
            return View(tariff);
        }

        public async Task<IActionResult> Edit(String id)
        {
            var TariffFromDb = await _db.Tariffs.FindAsync(id);
            return TariffFromDb == null ?
                NotFound() : View(TariffFromDb);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                _db.Tariffs.Update(tariff);
                await _db.SaveChangesAsync();
                TempData["success"] = "Tariff Updated Correctly";
                return RedirectToAction("Index");
            }
            return View(tariff);
        }

    }
}

