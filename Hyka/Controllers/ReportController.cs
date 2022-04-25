using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using Hyka.Data;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace Hyka.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReportController(ApplicationDbContext db) => _db = db;

        public IActionResult CreateDocument()
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

            //PdfColumnCollection tableColumns = pdfGrid.Columns;
            table.Columns.Add("Nombre");
            table.Columns.Add("Municipio");
            table.Columns.Add("Tipo de usuario");
            table.Columns.Add("Valor a pagar");
            table.Columns.Add("Fecha de registro");

            var people = _db.Users.Select(person => new { person.FullName, person.Municipality, person.TariffId, person.RegisterDateTime.Date }).ToList();

            foreach (var person in people)
            {
                var tariff = _db.Tariff.Find(person.TariffId);
                Object[] obj = { person.FullName, person.Municipality, tariff.Name, tariff.Price, person.Date };
                table.Rows.Add(obj);
            }
            pdfGrid.DataSource = table;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new PointF(30, 30));
            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            //Set the position as '0'.
            stream.Position = 0;
            //Close the document.
            document.Close(true);
            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = "Report_" + DateTime.Now + ".pdf";
            return fileStreamResult;
        }
    }
}