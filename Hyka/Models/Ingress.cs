using System.ComponentModel.DataAnnotations;

namespace Hyka.Models
{
    public class Ingress
    {
        public Ingress() { }

        public Barcode Barcode { get; set; }

        public Manual Manual { get; set; }

        public Group Group { get; set; }

    }
    public class Barcode
    {
        public Barcode() { }

        [Required]
        public String Code { get; set; }

    }

    public class Manual
    {
        public Manual() { }

        public Person Person { get; set; }
    }

    public class Group
    {

        public Group() { }

        [Required]
        public static List<KeyValuePair<Person, Tariff>> PeopleList = new();
        [Required]
        public static int Total = 0;


    }
}