using System.Text.Json.Serialization;

namespace JuniperMarket.Models.Tax
{
    public class Jurisdictions
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("county")]
        public string County { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }
    }
}
