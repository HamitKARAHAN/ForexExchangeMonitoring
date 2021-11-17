using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.JsonModels
{
    public class Quote
    {
        [JsonProperty("ask")]
        public double Ask { get; set; }

        [JsonProperty("base_currency")]
        public string BaseCurrency { get; set; }

        [JsonProperty("quote_currency")]
        public string QuoteCurrency { get; set; }
    }
}
