using System.Text.Json.Serialization;

namespace Hyka.Dtos
{
    public class MessageDto
    {
        public MessageDto() { }

        [JsonPropertyName("Head")]
        public String Head { get; set; }

        [JsonPropertyName("Subject")]
        public String Subject { get; set; }

        [JsonPropertyName("Body")]
        public String Body { get; set; }

        [JsonPropertyName("Url")]
        public String Url { get; set; }

    }

}
