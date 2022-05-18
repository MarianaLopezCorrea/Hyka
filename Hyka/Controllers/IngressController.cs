using Hyka.Data;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Hyka.Areas.Identity.PoliciesDefinition;
using static Hyka.Models.Ingress;
using Hyka.Services;

namespace Hyka.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_BLOCKBUSTER)]
    public class IngressController : Controller
    {

        private readonly IIngressService _ingressService;

        public IngressController(IIngressService ingressService)
        {
            _ingressService = ingressService;
        }

        public IActionResult Ingress()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SaveManual(Manual manual)
        {
            if (ModelState.IsValid)
            {
                manual.Person.FullName = manual.Person.FullName.ToUpper();
                if (_ingressService.AddToGroup(manual.Person))
                    TempData["success"] = "User added correctly";
                else
                    TempData["error"] = "User already exists";
            }
            return RedirectToAction("Ingress");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Decode(Barcode barcode)
        {
            if (ModelState.IsValid)
            {
                var person = _ingressService.Decode(barcode);
                if (_ingressService.AddToGroup(person))
                    TempData["success"] = "User added correctly";
                else
                    TempData["error"] = "User already exists";
            }
            return RedirectToAction("Ingress");
        }

        public IActionResult SaveGroup()
        {
            if (ModelState.IsValid)
            {
                _ingressService.SaveGroup();
                TempData["success"] = "Order complete successful";
            }
            return RedirectToAction("Ingress");
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                TempData["error"] = "Invalid ID";
                return RedirectToAction("Ingress");
            }
            _ingressService.RemovePersonFromGroup(id);
            TempData["success"] = "Deleted successfully";
            return RedirectToAction("Ingress");
        }

    }
}
