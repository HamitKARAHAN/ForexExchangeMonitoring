using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<ForexCurrencyModel> GetCurrencies(DateTime now)
        {
            //Include fonksiyonunda eager loadaing yapamadığım için history basarken hata yaşıyorum
            if (now.Hour > 20)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                            /*.Include(c => c.FromCurrencyCode.CurrencyModelId)*/;
            }
            else if (now.Hour < 11)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day - 1, 18, 0, 0);

                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                            /*.Include(c => c.FromCurrencyCode.CurrencyModelId)*/;
            }
            else if (now.Minute > 30)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, now.Hour - 3, 30, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                            /*.Include(c => c.FromCurrencyCode.CurrencyModelId)*/;
            }
            else
            {

                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, now.Hour - 3, 0, 0);
                return _context.RealTimeCurrencyExchangeRates
                                                            .Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0)
                                                            /*.Include(c => c.FromCurrencyCode.CurrencyModelId)*/;
            }
            throw new NotImplementedException();
        }

        public IEnumerable<ForexCurrencyModel> getCurrencyRate(int FromcurrencyModelId,int toCurrencyModelId)
        {
            //var x = _context.RealTimeCurrencyExchangeRates.Where(c => (c.FromCurrencyCode.CurrencyModelId == FromcurrencyModelId) && (c.ToCurrencyCode.CurrencyModelId== toCurrencyModelId)).Include(c => c.FromCurrencyCode.CurrencyModelId);
            return _context.RealTimeCurrencyExchangeRates.
                                        Where(c => (c.FromCurrencyCode.CurrencyModelId == FromcurrencyModelId) && (c.ToCurrencyCode.CurrencyModelId == toCurrencyModelId))
                                       /* .Include(c => c.FromCurrencyCode.CurrencyModelId)*/;
        }
    }
}
