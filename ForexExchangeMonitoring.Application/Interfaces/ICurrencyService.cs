using ForexExchangeMonitoring.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Application.Interfaces
{
    public interface ICurrencyService
    {
        CurrenciesRateViewModel GetLiveCurrencies();
        CurrenciesRateViewModel GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);

        CurrenciesRateViewModel GetCurrencies();
    }
}
