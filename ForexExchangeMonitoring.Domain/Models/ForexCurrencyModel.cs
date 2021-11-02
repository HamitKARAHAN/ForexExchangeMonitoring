using System;

namespace ForexExchangeMonitoring.Domain
{
    public class ForexCurrencyModel
    {
        public int ForexCurrencyModelId { get; set; }
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }
        public DateTime LastRefreshedDate { get; set; }

    }
}
