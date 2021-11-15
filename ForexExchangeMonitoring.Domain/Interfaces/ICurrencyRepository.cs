﻿using ForexExchangeMonitoring.Domain.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        List<CurrencyModel> GetCurrencies();
        IEnumerable<ForexCurrencyRateModel> GetLiveCurrencies();
        IEnumerable<HistoryRateModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId);

        public void AddLiveExchangeRate(ForexCurrencyRateModel live);

        public void AddHistoryExchangeRate(HistoryRateModel history);

        public void UpdateDb(ForexCurrencyRateModel live);
        public void SaveToDb();
    }
}
