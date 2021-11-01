using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForexExchange.Models
{
    public class RealTimeCurrencyExchangeRate
    {
        public int RealTimeCurrencyExchangeRateId { get; set; }
        public string FromCurrencyCode{ get; set; }
        public string ToCurrencyCode { get; set; }
        public DateTime LastRefreshedDate { get; set; }
    }
}
