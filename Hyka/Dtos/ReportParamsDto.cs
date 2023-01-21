using System.ComponentModel.DataAnnotations;

namespace Hyka.Models
{
    public class ReportParamsDto
    {
        [Required]
        public string Type { get; set; }

        public string Period { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsFiltered { get; set; }

    }



}