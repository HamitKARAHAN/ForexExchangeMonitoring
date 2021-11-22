using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.ViewModels;
using ForexExchangeMonitoring.Domain.DbModels;
using ForexExchangeMonitoring.Domain.Interfaces;
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

        public CurrenciesViewModel GetCurrencies()
        {
            return new CurrenciesViewModel()
            {
                Currencies = _currencyRepository.GetCurrencies()
            };
        }

        public LiveCurrenciesRateViewModel GetLiveCurrenciesBySort(string sortOrder)
        {
            return new LiveCurrenciesRateViewModel()
            {
                ForexLiveCurrencies = _currencyRepository.GetLiveCurrenciesBySort(sortOrder)
            };
        }

        public LiveCurrenciesRateViewModel GetLiveCurrenciesBySearch(string from, string to, string minRate)
        {
            return new LiveCurrenciesRateViewModel()
            {
                ForexLiveCurrencies = _currencyRepository.GetLiveCurrenciesBySearch(from, to, minRate)
            };
        }

        public CurrenciesHistoryRateViewModel GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return new CurrenciesHistoryRateViewModel()
            {
                ForexHistory = _currencyRepository.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId)
            };
        }
    }
}

