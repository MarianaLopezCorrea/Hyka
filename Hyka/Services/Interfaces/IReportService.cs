using Hyka.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using System.Data;

namespace Hyka.Services
{
    public interface IReportService
    {
        DataColumn[] REPORT_COLUMNS { get; set; }
        PdfDocument ExportToPdf(Report param);
        FileStreamResult DownloadPdf(PdfDocument document);

    }
}
