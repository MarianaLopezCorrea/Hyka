using System.ComponentModel.DataAnnotations;

namespace Hyka.Models
{
    public class Report
    {
        
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public IEnumerable<History> ReportData { get; set; }

    }



}