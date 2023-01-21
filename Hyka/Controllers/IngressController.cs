using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using Microsoft.AspNetCore.Authorization;

using Hyka.Services;
using Hyka.Extensions;
using System.Text.Json;
using Hyka.Dtos;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Hyka.Areas.Identity.RolesDefinition;

namespace Hyka.Controllers
{
    [Authorize(Roles = $"{Roles.BLOCKBUSTER}")]
    [AutoValidateAntiforgeryToken]
    public class IngressController : Controller
    {

        private readonly IIngressService _ingressService;
        private readonly ITariffService _tariffService;

        public IngressController(IIngressService ingressService, ITariffService tariffService)
        {
            _ingressService = ingressService;
            _tariffService = tariffService;
        }

        public IActionResult Ingress()
        {
            _setSessionVaribles(HttpContext.Session);
            return View();
        }

        [HttpPost]
        public IActionResult SaveManually(PersonDto personDto)
        {
            if (ModelState.IsValid)
            {
                if (_ingressService.AddToIngressList(HttpContext.Session, personDto))
                    TempData["status"] = JsonSerializer.Serialize(new { type = "success", message = "Lista de ingreso actualizada" });
                else
                    TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "Error, por favor, intente de nuevo" });
            }
            return RedirectToAction("Ingress");
        }

        [HttpPost]
        public IActionResult Decode(String barcode)
        {
            if (ModelState.IsValid)
            {
                var personDto = _ingressService.Decode(barcode);

                if (personDto == null)
                {
                    TempData["status"] = JsonSerializer.Serialize(new
                    {
                        type = "error",
                        message = "Código PDF417 inválido o Documento no encontrado"
                    });
                    return RedirectToAction("Ingress");
                }
                if (_ingressService.AddToIngressList(HttpContext.Session, personDto))
                    TempData["status"] = JsonSerializer.Serialize(new { type = "success", message = "Lista de ingreso actualizada" });
                else
                    TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "Error, por favor, intente de nuevo" });
            }
            return RedirectToAction("Ingress");
        }

        [HttpPost]
        public IActionResult SaveIngressList()
        {
            if (ModelState.IsValid)
            {
                _ingressService.SaveIngressList(HttpContext);
                TempData["status"] = JsonSerializer.Serialize(new { type = "success", message = "Lista guardada exitosamente" });
            }
            return RedirectToAction("Ingress");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "ID inválida" });
                return RedirectToAction("Ingress");
            }
            if (_ingressService.RemovePersonFromIngressList(HttpContext.Session, id))
                TempData["status"] = JsonSerializer.Serialize(new { type = "success", message = "Lista de ingreso actualizada" });
            else
                TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "El usuario no se encuentra en la lista" });

            return RedirectToAction("Ingress");
        }


        [HttpGet]
        public IActionResult HasPet(string id)
        {
            if (id == null)
            {
                TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "ID inválida" });
                return RedirectToAction("Ingress");
            }
            if (!_ingressService.HasPet(HttpContext.Session, id))
                TempData["status"] = JsonSerializer.Serialize(new { type = "error", message = "ID inválida" });

            return RedirectToAction("Ingress");
        }

        private void _setSessionVaribles(ISession session)
        {
            var ingressList = session.GetObject<List<KeyValuePair<PersonDto, Tariff>>>("INGRESS_LIST");
            int? total = session.GetObject<Int32>("TOTAL");
            if (ingressList == null)
                session.SetObject("INGRESS_LIST", new List<KeyValuePair<PersonDto, Tariff>>());
            if (total == null)
                session.SetObject("TOTAL", 0);
        }

    }
}
