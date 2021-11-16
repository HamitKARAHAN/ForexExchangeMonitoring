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
        List<CurrencyModel> GetCurrencies();
        IEnumerable<ForexCurrencyRateModel> GetLiveCurrencies(string sortOrder, string fromCurrencySerachString, string toCurrencySerachString, string rateCurrencySearchString);
        IEnumerable<HistoryRateModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);
    }
}
