using Microsoft.AspNetCore.Mvc;
using System.Data;
using Hyka.Models;
using Hyka.Services;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

using Hyka.Areas.Identity.RolesDefinition;

namespace Hyka.Controllers
{
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.BLOCKBUSTER}")]
    [AutoValidateAntiforgeryToken]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateReport(ReportParamsDto rParams)
        {
            if (ModelState.IsValid)
            {
                var report = _reportService.GetJsonReport(rParams);
                if (report == "[]")
                {
                    TempData["status"] = JsonSerializer.Serialize(new { type = "warning", message = "No existen registros para las fechas solictadas" });
                }
                else
                {
                    TempData["report"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(report));
                    TempData["status"] = JsonSerializer.Serialize(new { type = "info", message = "Buscando datos" });
                }
                return RedirectToAction("Report");
            }
            return View("Report", rParams);
        }

    }
}
