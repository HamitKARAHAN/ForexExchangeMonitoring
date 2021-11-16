using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.ViewModels;
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

        public LiveCurrenciesRateViewModel GetLiveCurrencies()
        {
            return new LiveCurrenciesRateViewModel()
            {
                ForexLiveCurrencies = _currencyRepository.GetLiveCurrencies()
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

