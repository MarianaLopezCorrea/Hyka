using System.Text.Json;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Models;
using Hyka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Controllers
{
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
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                if (_tariffService.Update(tariff))
                {
                    TempData["status"] = JsonSerializer.Serialize(new { type = "success", message = "Tarifa actualizada correctamente" });
                    return RedirectToAction("Index");
                }
                TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "Error al actualizar" });
                return View("Index");
            }
            return View("Index");
        }

    }
}

