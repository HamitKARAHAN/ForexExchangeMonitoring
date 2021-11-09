using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.Models;
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





            if (now.Hour > 20)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            else if (now.Hour < 11)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day - 1, 18, 0, 0);

                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            else if (now.Minute > 30)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, now.Hour - 3, 30, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                           .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            else
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, now.Hour - 3, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                            .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
        }

        public IEnumerable<ForexCurrencyModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return _context.RealTimeCurrencyExchangeRates.
                                        Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId) && (c.ToCurrency.CurrencyModelId == toCurrencyModelId))
                                        .Include(c => c.FromCurrency).Include(c => c.ToCurrency);

        }
    }
}
