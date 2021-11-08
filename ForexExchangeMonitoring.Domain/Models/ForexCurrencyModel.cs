using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexExchangeMonitoring.Domain.Models
{
    public class ForexCurrencyModel
    {
        public int ForexCurrencyModelId { get; set; }

        [JsonProperty("1. From_Currency Code")]
        [NotMapped]
        public string _FromCurrencyCode { get; set; }
        public CurrencyModel FromCurrencyCode { get; set; }

        [JsonProperty("3. To_Currency Code")]
        [NotMapped]
        public string _ToCurrencyCode { get; set; }
        public CurrencyModel ToCurrencyCode { get; set; }

        [JsonProperty("5. Exchange Rate")]
        public string ExchangeRate { get; set; }

        [JsonProperty("6. Last Refreshed")]
        public DateTime LastRefreshedDate { get; set; }
    }
}
