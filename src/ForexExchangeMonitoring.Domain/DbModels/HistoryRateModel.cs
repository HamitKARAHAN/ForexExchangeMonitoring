using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.DbModels
{
    public class HistoryRateModel
    {
        [Key]
        [Column("currency_exchange_id")]
        public int HistoryId { get; set; }

        public int? FromCurrencyId { get; set; }
        public CurrencyModel FromCurrency { get; set; }

        public int? ToCurrencyId { get; set; }
        public CurrencyModel ToCurrency { get; set; }

        [Column("exchange_rate")]
        public double ExchangeRate { get; set; }

        [Column("last_refreshed_date")]
        public DateTime LastRefreshedDate { get; set; }
        public ForexCurrencyRateModel ForexCurrencyModel { get; set; }
    }
}
