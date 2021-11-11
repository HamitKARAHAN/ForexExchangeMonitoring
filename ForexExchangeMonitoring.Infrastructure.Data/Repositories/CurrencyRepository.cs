using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForexExchangeMonitoring.Infrastructure.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ForexCurrencyModelDbContext _context;
        public CurrencyRepository(ForexCurrencyModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ForexCurrencyModel> GetCurrencies(DateTime now)
        {

            if (now.Hour >= 18)
            {
                DateTime lastInsertDate = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(lastInsertDate) > 0)
                                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            else if (now.Hour < 9)
            {
                DateTime lastInsertDate = new DateTime(now.Year, now.Month, now.Day - 1, 18, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(lastInsertDate) > 0)
                                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            else if (now.Minute >= 30)
            {
                DateTime lastInsertDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, 30, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(lastInsertDate) > 0)
                                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            else
            {
                DateTime lastInsertDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(lastInsertDate) > 0)
                                                            .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
        }

        public IEnumerable<ForexCurrencyModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {

            DateTime now = DateTime.Now;

            if(now.Hour>=0 && now.Hour<9)
            {
                DateTime lastInsertDate = new DateTime(now.Year, now.Month, now.Day - 1, 9, 0, 0);
                var x = _context.RealTimeCurrencyExchangeRates
                                           .Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId) && (c.ToCurrency.CurrencyModelId == toCurrencyModelId) && (c.LastRefreshedDate.CompareTo(lastInsertDate) >= 0))
                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            return _context.RealTimeCurrencyExchangeRates
                                        .Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId) && (c.ToCurrency.CurrencyModelId == toCurrencyModelId) && (c.LastRefreshedDate.Day==now.Day))
                                        .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
        }
    }
}
