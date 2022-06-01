using Hyka.Areas.Identity.PoliciesDefinition;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Data;
using Hyka.Models;
using Hyka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_ADMIN)]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public class TariffController : Controller
    {
        private readonly ITariffService _tariffService;

        public TariffController(ITariffService tariffService)
        {
            _tariffService = tariffService;
        }

        public IActionResult Index()
        {
            return View(_tariffService.Get());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Tariff tariff)
        {
            var result = _tariffService.GetById(tariff.Id);
            if (result != null)
            {
                ModelState.AddModelError(string.Empty, "Tariff already exist");
                return View();
            }
            if (ModelState.IsValid)
            {
                _tariffService.Create(tariff);
                TempData["success"] = "Tariff Created Correctly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Invalid tariff";
            return View(tariff);
        }

        public ActionResult Edit(Guid id)
        {
            var tariff = _tariffService.GetById(id);
            if (tariff == null)
            {
                TempData["error"] = "Tariff don't found";
                return RedirectToAction("Edit");
            }
            return View(tariff);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Tariff tariff)
        {
            var result = _tariffService.GetById(tariff.Id);
            if (result == null)
            {
                TempData["error"] = "Tariff don't found";
                return RedirectToAction("Edit");
            }
            if (ModelState.IsValid)
            {
                _tariffService.Update(tariff);
                TempData["success"] = "Tariff Updated Correctly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Invalid tariff";
            return View(tariff);
        }

        public ActionResult Delete(Guid id)
        {
            if (_tariffService.Delete(id))
            {
                TempData["success"] = "Tariff Deleted Correctly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error removing tariff";
            return RedirectToAction("Index");
        }

    }
}

