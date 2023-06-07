using Newtonsoft.Json;

namespace RatesApi.DTOs
{
    public class RatesDTO
    {
        [JsonProperty("cc")]
        public string Name { get; set; }
        [JsonProperty("rate")]

        public double Rate { get; set; }
        [JsonProperty("exchangedate")]

        public DateTime ExchangeRate { get; set; }
    }
}
