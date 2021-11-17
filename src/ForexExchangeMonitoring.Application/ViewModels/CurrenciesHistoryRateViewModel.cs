using ForexExchangeMonitoring.Domain.DbModels;
using System;
using System.Collections.Generic;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class CurrenciesHistoryRateViewModel
    {
        public IEnumerable<HistoryRateModel> ForexHistory { get; set; }
    }
}
