using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyka.Service.Controllers;

namespace Hyka.Models
{
    public class Decoder
    {

        public String Code { get; set; }


        public Person Decode()
        {
            Regex regexRh = new Regex(@"(A|B|O|AB)(\+|-)");
            Match rhmatch = regexRh.Match(Code);
            Person person = new Person();
            if (rhmatch.Success)
            {
                person.DocumentType = Code.ElementAt(0).Equals('I') ? "IT" : "CC";
                person.BloodType = rhmatch.Value;
                Code = Code.Substring(0, rhmatch.Index);
                person.Municipality = Code.Substring(Code.Length - 4, 3);
                person.Department = Code.Substring(Code.Length - 6, 2);
                person.Age = DateTime.UtcNow.Year - Int32.Parse(Code.Substring(Code.Length - 14, 4));
                person.Gender = Code.Substring(Code.Length - 15, 1);
                int i = 0;
                for (i = Code.Length - 17; i > 0; i--)
                {
                    if (Char.IsDigit(Code[i]))
                    {
                        break;
                    }
                }
                String fullName = Code.Substring(i + 1, Code.Length - 17 - i);
                person.FullName = Regex.Replace(fullName, @"([^A-ZÑ])+", " ").TrimEnd();
                person.Id = Code.Substring(i - 9, 10);
                var territory = Territory.territories.Where(
                TerritoryDto =>
                    TerritoryDto.DepartmentId == person.Department &&
                    TerritoryDto.MunicipalityID == person.Municipality
                )
                .FirstOrDefault();
                person.Department = territory.DepartmentName;
                person.Municipality = territory.MunicipalityName;

                // HttpClient client = new HttpClient();
                // Uri uri = new Uri("https://localhost:7281/territories/" + person.Department + "/" + person.Municipality);
                //https://makolyte.com/csharp-get-and-send-json-with-httpclient/
                // var territory = client.GetFromJsonAsync<Territory>($"https://localhost:7281/territories/{person.Department}/{person.Municipality}");

                // client.BaseAddress = new Uri("https://localhost:7281/territories/" + person.Department + "/" + person.Municipality);
                // client.DefaultRequestHeaders.Accept.Clear();
                // client.Method = "POST";
                // client.ContentLength = data.Length;
                // client.DefaultRequestHeaders.Accept.Add(
                // new MediaTypeWithQualityHeaderValue("application/json"));
                // Console.WriteLine(client);
                //string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("usuario:clave"));
                //client.Headers.Add("Authorization", "Basic " + credentials);


                JsonSerializerOptions jOptions = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                String jsonString = JsonSerializer.Serialize(person, jOptions);
                Console.WriteLine(jsonString);
            }

            return person;
        }

    }
}