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
        ForexCurrencyViewModel GetCurrencies(DateTime now);
        ForexCurrencyViewModel GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);
    }
}
