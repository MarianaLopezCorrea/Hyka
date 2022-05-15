using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using Hyka.Data;
using Syncfusion.Pdf.Grid;
using System.Data;
using Hyka.Models;

namespace Hyka.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReportController(ApplicationDbContext db) => _db = db;

        public IActionResult Sales()
        {
            return View();
        }

        public IActionResult Ingress()
        {
            return View();
        }

        public IActionResult SalesReport(RangeDate range)
        {
            if (ModelState.IsValid)
            {
                //Create a new PDF document
                PdfDocument document = new PdfDocument();
                //Add a page to the document
                PdfPage page = document.Pages.Add();
                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;
                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
                //Draw the text 
                graphics.DrawString("Reporte de ingresos", font, PdfBrushes.Black, new PointF(0, 0));
                //Create a PdfGrid.
                PdfGrid pdfGrid = new PdfGrid();
                DataTable table = new DataTable();
                table.Columns.Add("Tipo de usuario");
                table.Columns.Add("Valor a pagar");
                table.Columns.Add("Fecha de registro");
                var people = _db.Histories.Where(
                    h => h.VisitDateTime.Date >= range.Start.Date && h.VisitDateTime.Date <= range.End.Date
                );
                var total = people.Sum(h => h.Price);
                foreach (var person in people)
                {
                    table.Rows.Add(person.TariffName, person.Price, person.VisitDateTime);
                }
                table.Rows.Add("Total", total);
                pdfGrid.DataSource = table;
                //Draw grid to the page of PDF document.
                pdfGrid.Draw(page, new PointF(30, 30));
                //Saving the PDF to the MemoryStream
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;
                //Close the document.
                document.Close(true);
                //Download the PDF document in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
                fileStreamResult.FileDownloadName = "Report_" + DateTime.Now.ToShortDateString() + ".pdf";
                return fileStreamResult;
            }
            return View();
        }
        public IActionResult IngressReport(RangeDate range)
        {
            if (ModelState.IsValid)
            {
                //Create a new PDF document
                PdfDocument document = new PdfDocument();
                //Add a page to the document
                PdfPage page = document.Pages.Add();
                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;
                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
                //Draw the text 
                graphics.DrawString("Reporte de ingresos", font, PdfBrushes.Black, new PointF(0, 0));
                //Create a PdfGrid.
                PdfGrid pdfGrid = new PdfGrid();
                DataTable table = new DataTable();
                table.Columns.Add("Tipo de usuario");
                table.Columns.Add("Valor a pagar");
                table.Columns.Add("Fecha de registro");
                var people = _db.Histories.Where(
                    h => h.VisitDateTime.Date >= range.Start.Date && h.VisitDateTime.Date <= range.End.Date
                );
                var total = people.Sum(h => h.Price);
                foreach (var person in people)
                {
                    table.Rows.Add(person.TariffName, person.Price, person.VisitDateTime);
                }
                table.Rows.Add("Total", total);
                pdfGrid.DataSource = table;
                //Draw grid to the page of PDF document.
                pdfGrid.Draw(page, new PointF(30, 30));
                //Saving the PDF to the MemoryStream
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;
                //Close the document.
                document.Close(true);
                //Download the PDF document in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
                fileStreamResult.FileDownloadName = "Report_" + DateTime.Now.ToShortDateString() + ".pdf";
                return fileStreamResult;
            }
            return View();
        }
    }
}