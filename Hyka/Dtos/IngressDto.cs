using System.ComponentModel.DataAnnotations;

namespace Hyka.Dtos
{
    public class IngressDto
    {
        public IngressDto() { }

        [Required]
        public String Barcode { get; set; }

        public PersonDto PersonDto { get; set; }

    }
}
