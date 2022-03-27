using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyka.Models
{
    public class Blockbuster
    {
        // Equivalent to Primary Key in SQL
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        // Equivalent to NOT NULL
        [Required]
        public String Names { get; set; }
        [Required]
        public String Surnames { get; set; }
        // Displayed in error handler
        [Range(18, 70, ErrorMessage = "Debe tener entre 18 y 70 años")]
        public int Age { get; set; }
        [Required]
        public String Gender { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now; // Default value when object is created
        [EmailAddress]
        public String Email { get; set; }
        [PasswordPropertyText]
        public String Password { get; set; }
    }

}
