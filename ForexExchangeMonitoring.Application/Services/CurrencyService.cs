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

        public CurrenciesRateViewModel GetCurrencies()
        {
            return new CurrenciesRateViewModel()
            {
                Currencies = _currencyRepository.GetCurrencies()
            };
        }

        public CurrenciesRateViewModel GetLiveCurrencies()
        {
            return new CurrenciesRateViewModel()
            {
                ForexLiveCurrencies = _currencyRepository.GetLiveCurrencies()
            };
        }

        CurrenciesRateViewModel ICurrencyService.GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return new CurrenciesRateViewModel()
            {
                ForexHistory = _currencyRepository.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId)
            };
        }


    }
}

