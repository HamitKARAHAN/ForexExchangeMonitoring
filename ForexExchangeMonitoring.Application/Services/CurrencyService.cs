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

        public ForexCurrencyViewModel GetCurrencies(DateTime now)
        {
            return new ForexCurrencyViewModel()
            {
                ForexCurrencies = _currencyRepository.GetCurrencies(now)
            };
        }
        ForexCurrencyViewModel ICurrencyService.GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return new ForexCurrencyViewModel()
            {
                ForexCurrencies = _currencyRepository.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId)
            };
        }
    }
}

