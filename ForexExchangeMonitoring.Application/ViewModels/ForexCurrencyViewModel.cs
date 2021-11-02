using ForexExchangeMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Application.ViewModels
{
    public class ForexCurrencyViewModel
    {
        public IEnumerable<ForexCurrencyModel> Currencies { get; set; }
    }
}
