using ForexExchangeMonitoring.Domain.DbModels;
using System.Collections.Generic;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class LiveCurrenciesRateViewModel
    {
        public IEnumerable<ForexCurrencyRateModel> ForexLiveCurrencies { get; set; }
    }
}
