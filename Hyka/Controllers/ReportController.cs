using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using Hyka.Data;
using Syncfusion.Pdf.Grid;
using System.Data;
using Hyka.Models;
using Hyka.Services;

namespace Hyka.Controllers
{
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
        public IActionResult GenerateReport(Report param)
        {
            if (ModelState.IsValid)
            {
                var doc = _reportService.ExportToPdf(param);
                return _reportService.DownloadPdf(doc);
            }
            return BadRequest();
        }

    }
}