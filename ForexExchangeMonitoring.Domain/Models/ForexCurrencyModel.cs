using Newtonsoft.Json;
using System;

namespace ForexExchangeMonitoring.Domain.Models
{
    public class ForexCurrencyModel
    {
        public int ForexCurrencyModelId { get; set; }

        [JsonProperty("1. From_Currency Code")]
        public string FromCurrencyCode { get; set; }

        [JsonProperty("3. To_Currency Code")]
        public string ToCurrencyCode { get; set; }

        [JsonProperty("5. Exchange Rate")]
        public string ExchangeRate { get; set; }

        [JsonProperty("6. Last Refreshed")]
        public DateTime LastRefreshedDate { get; set; }

    }
}
