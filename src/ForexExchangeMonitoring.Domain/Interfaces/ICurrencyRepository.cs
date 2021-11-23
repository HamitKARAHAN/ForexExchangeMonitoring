using ForexExchangeMonitoring.Domain.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        public void AddLiveExchangeRate(ForexCurrencyRateModel live);
        public void AddHistoryExchangeRate(HistoryRateModel history);
        public void UpdateDb(ForexCurrencyRateModel live);
        public void SaveToDb();
        IEnumerable<CurrencyModel> GetCurrencies();
        IEnumerable<ForexCurrencyRateModel> GetLiveCurrencies();
        IEnumerable<ForexCurrencyRateModel> GetLiveCurrenciesBySort(string sortOrder);
        IEnumerable<ForexCurrencyRateModel> GetLiveCurrenciesBySearch(string from, string to, string minRate);
        IEnumerable<HistoryRateModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);
    }
}
