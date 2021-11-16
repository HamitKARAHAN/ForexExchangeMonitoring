using ForexExchangeMonitoring.Domain.DbModels;
using System;
using System.Collections.Generic;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class CurrenciesViewModel
    {
        public List<CurrencyModel> Currencies { get; set; }
    }
}
