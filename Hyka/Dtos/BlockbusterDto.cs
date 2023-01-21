using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Hyka.Models
{
    public class Blockbuster
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "El nombre debe contener solo letras y no espacios")]
        public String UserName { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

    }

}
