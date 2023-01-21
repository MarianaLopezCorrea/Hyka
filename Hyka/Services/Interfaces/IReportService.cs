using Hyka.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using System.Data;

namespace Hyka.Services
{
    public interface IReportService
    {
        String GetJsonReport(ReportParamsDto reportParams);
    }
}
