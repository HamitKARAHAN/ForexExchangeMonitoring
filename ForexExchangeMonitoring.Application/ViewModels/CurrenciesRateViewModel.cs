using ForexExchangeMonitoring.Domain.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class CurrenciesRateViewModel
    {
        public IEnumerable<ForexCurrencyRateModel> ForexLiveCurrencies { get; set; }
        public IEnumerable<HistoryRateModel> ForexHistory { get; set; }
        public List<CurrencyModel> Currencies { get; set; }
    }
}
