using ForexExchangeMonitoring.Domain.Models;
using System.Collections.Generic;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class ForexCurrencyViewModel
    {
        public IEnumerable<ForexCurrencyModel> ForexCurrencies { get; set; }
    }
}
