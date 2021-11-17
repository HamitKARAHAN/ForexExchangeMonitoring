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
        LiveCurrenciesRateViewModel GetLiveCurrenciesBySort(string sortOrder);
        LiveCurrenciesRateViewModel GetLiveCurrenciesBySearch(string from, string to, string minRate);
        CurrenciesHistoryRateViewModel GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);
        CurrenciesViewModel GetCurrencies();
    }
}
