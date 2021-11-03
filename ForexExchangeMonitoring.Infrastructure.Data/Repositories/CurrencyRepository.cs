using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Infrastructure.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public ForexCurrencyModelDbContext _context;
        public CurrencyRepository(ForexCurrencyModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ForexCurrencyModel> GetCurrencies()
        {
            return _context.RealTimeCurrencyExchangeRates;
        }

        public void WriteToDb(ForexCurrencyModel model)
        {
            _context.RealTimeCurrencyExchangeRates.Add(model);
            _context.SaveChanges();
        }
    }
}
