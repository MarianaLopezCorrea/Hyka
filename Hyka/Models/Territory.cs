using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Territory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String DaneId { get; set; }
        [Required]
        public String MunicipalityName { get; set; }
        [Required]
        public String DepartmentName { get; set; }

        public Territory() { }
        public Territory(String daneId, String municipalityName, String departmentName)
        {
            this.DaneId = daneId;
            this.MunicipalityName = municipalityName;
            this.DepartmentName = departmentName;
        }

        public record TerritoryRecord(string DaneId, string DepartmentName, string MunicipalityName);

    }
}
