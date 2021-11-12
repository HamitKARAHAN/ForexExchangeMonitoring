using ForexExchangeMonitoring.Domain.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class LiveCurrenciesRateViewModel
    {
        public IEnumerable<ForexCurrencyRateModel> ForexLiveCurrencies { get; set; }
        public IEnumerable<HistoryRateModel> ForexHistory { get; set; }
    }
}
