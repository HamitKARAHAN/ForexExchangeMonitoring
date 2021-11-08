using ForexExchangeMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        IEnumerable<ForexCurrencyModel> GetCurrencies(DateTime now);
        IEnumerable<ForexCurrencyModel> getCurrencyRate(int fromCurrencyModelId,int toCurrencyModelId);
        //void WriteToDb(ForexCurrencyModel model);
    }
}
