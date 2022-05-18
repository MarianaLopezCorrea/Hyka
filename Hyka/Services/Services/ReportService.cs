using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using Hyka.Data;
using Syncfusion.Pdf.Grid;
using System.Data;
using Hyka.Models;

namespace Hyka.Services
{
    public class ReportService : IReportService
    {

        public DataColumn[] REPORT_COLUMNS { get; set; } =
        {
            new DataColumn("Categoría"),
            new DataColumn("Cantidad"),
            new DataColumn("Recaudado")
        };

        private readonly IHistoryService _historyService;

        public ReportService(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        public FileStreamResult DownloadPdf(PdfDocument document)
        {
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

        public PdfDocument ExportToPdf(Report param)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            //Report Title
            graphics.DrawString("Park report", font, PdfBrushes.Black, new PointF(0, 0));
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            DataTable table = new DataTable();
            table.Columns.AddRange(REPORT_COLUMNS);
            var history = _historyService.GetBetween(param.StartDate, param.EndDate)
                            .GroupBy(r => r.TariffName)
                            .Select(g => new
                            {
                                category = g.Key,
                                count = g.Count(),
                                collect = g.Sum(r => r.Price)
                            });
            var subtotal = history.Sum(r => r.count);
            var total = history.Sum(r => r.collect);
            foreach (var record in history)
            {
                table.Rows.Add(record.category, record.count, record.collect);
            }
            table.Rows.Add("Total", subtotal, total);
            pdfGrid.DataSource = table;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new PointF(30, 30));
            //Close the document.
            return document;
        }
    }
}