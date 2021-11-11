using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.DbModels
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

        public double ExchangeRate { get; set; }
        public DateTime LastRefreshedDate { get; set; }

    }
}
