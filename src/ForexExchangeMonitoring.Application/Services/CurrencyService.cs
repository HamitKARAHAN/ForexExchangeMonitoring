using ForexExchangeMonitoring.Application.Helpers;
using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.ViewModels;
using ForexExchangeMonitoring.Domain.DbModels;
using ForexExchangeMonitoring.Domain.Interfaces;
using Log;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public LiveCurrenciesRateViewModel GetLiveCurrenciesBySort(string sortOrder)
        {
            var forexLiveCurrencies = _currencyRepository.GetLiveCurrenciesBySort(sortOrder);
            if (forexLiveCurrencies == null)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Message = "Application.GetLiveCurrenciesBySort", MessageDetail = "There is No Live Data" });
                throw new ArgumentNullException("Live Data is Empty");
            }
            return new LiveCurrenciesRateViewModel()
            {
                ForexLiveCurrencies = forexLiveCurrencies
            };
        }

        public LiveCurrenciesRateViewModel GetLiveCurrenciesBySearch(string from, string to, string minRate)
        {
            var forexLiveCurrencies = _currencyRepository.GetLiveCurrenciesBySearch(from, to, minRate);
            if (forexLiveCurrencies == null)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Message = "Application.GetLiveCurrenciesBySearch", MessageDetail = "There is No Live Data" });
                throw new ArgumentNullException("Live Data is Empty");
            }
            return new LiveCurrenciesRateViewModel()
            {
                ForexLiveCurrencies = forexLiveCurrencies
            };
        }

        public CurrenciesHistoryRateViewModel GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            var forexHistory = _currencyRepository.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId);
            if (forexHistory == null)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Message = "Application.GetLiveCurrenciesBySort", MessageDetail = "There is No History Data" });
                throw new ArgumentNullException("History Data is Empty");
            }
            return new CurrenciesHistoryRateViewModel()
            {
                ForexHistory = forexHistory
            };
        }
    }
}

