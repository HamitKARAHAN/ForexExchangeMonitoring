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
        LiveCurrenciesRateViewModel GetLiveCurrencies(string sortOrder, string fromCurrencySerachString, string toCurrencySerachString, string rateCurrencySearchString);
        CurrenciesHistoryRateViewModel GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);
        CurrenciesViewModel GetCurrencies();
    }
}
