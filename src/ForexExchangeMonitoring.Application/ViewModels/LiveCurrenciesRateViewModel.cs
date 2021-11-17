using ForexExchangeMonitoring.Domain.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class LiveCurrenciesRateViewModel
    {
        public IEnumerable<ForexCurrencyRateModel> ForexLiveCurrencies { get; set; }
    }
}
