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

        public IEnumerable<ForexCurrencyModel> GetCurrencies()
        {
            DateTime now = DateTime.Now;
            if (now.Hour > 20)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
                var result = _context.RealTimeCurrencyExchangeRates.Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0);
            }
            else if (now.Hour < 11)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day - 1, 18, 0, 0);
                var result = _context.RealTimeCurrencyExchangeRates.Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0);
            }
            else if (now.Minute > 30)
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, now.Hour - 2, 30, 0);
                var result = _context.RealTimeCurrencyExchangeRates.Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0);
            }
            else
            {
                DateTime queryDate = new DateTime(now.Year, now.Month, now.Day, now.Hour - 2, 0, 0);
                var result = _context.RealTimeCurrencyExchangeRates.Where(c => c.LastRefreshedDate.CompareTo(queryDate) > 0);
            }




            //foreach (var item in result)
            //{

            //}

            return _context.RealTimeCurrencyExchangeRates.Include(c => c.currencyModelId);
        }

        //public void WriteToDb(ForexCurrencyModel model)
        //{
        //    _context.RealTimeCurrencyExchangeRates.Add(model);
        //    _context.SaveChanges();
        //}
    }
}
