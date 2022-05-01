using System.ComponentModel.DataAnnotations;

namespace Hyka.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public String Password { get; set; }
    }
}