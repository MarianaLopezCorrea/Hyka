using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using Hyka.Data;
using Syncfusion.Pdf.Grid;
using System.Data;
using Hyka.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Hyka.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _db;

        private readonly IHistoryService _historyService;

        private readonly IPersonService _personService;

        public ReportService(ApplicationDbContext db, IHistoryService historyService, IPersonService personService)
        {
            _db = db;
            _historyService = historyService;
            _personService = personService;
        }

        public string GetJsonReport(ReportParamsDto rParams)
        {
            var pair = rParams.Type + rParams.IsFiltered.ToString();
            var REPORT_DATA = new Dictionary<string, Func<String>>(){
                {
                    "PeopleTrue", () => _getJsonAsync(_personService.GetBetween(rParams.StartDate, rParams.EndDate)).Result
                },
                {
                    "HistoryTrue",() =>  _getJsonAsync(_historyService.GetBetween(rParams.StartDate, rParams.EndDate)).Result
                },
                {
                    "PeopleFalse", () => _getJsonAsync(_personService.GetAll()).Result
                },
                {
                    "HistoryFalse", () => _getJsonAsync(_historyService.GetAll()).Result
                }
            };
            return REPORT_DATA[pair].Invoke();
        }

        private static async Task<string> _getJsonAsync<T>(T obj)
        {
            string json = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync<T>(stream, obj);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                json = await reader.ReadToEndAsync();
            }
            return json;
        }


    }
}