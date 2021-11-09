using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ForexExchangeMonitoring.Domain.Models
{
    public class ForexCurrencyModel
    {
        [Key]
        [Column("forex_currency_model_id")]
        public int ForexCurrencyModelId { get; set; }

        public int FromCurrencyId { get; set; }
        public virtual CurrencyModel FromCurrency { get; set; }


        public int ToCurrencyId { get; set; }
        public virtual CurrencyModel ToCurrency { get; set; }


        [JsonProperty("5. Exchange Rate")]
        [Column("exchange_rate")]
        public string ExchangeRate { get; set; }

        [JsonProperty("6. Last Refreshed")]
        [Column("last_refreshed_date")]
        public DateTime LastRefreshedDate { get; set; }



        [JsonProperty("1. From_Currency Code")]
        [NotMapped]
        public string FromCurrencyCode { get; set; }

        [JsonProperty("3. To_Currency Code")]
        [NotMapped]
        public string ToCurrencyCode { get; set; }
    }
}
