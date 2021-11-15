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

        public void AddLiveExchangeRate(ForexCurrencyRateModel live)
        {
            _context.CurrencyExchangeRatesLive.Add(live);
        }

        public void AddHistoryExchangeRate(HistoryRateModel history)
        {
            _context.CurrencyExchangeRatesHistory.Add(history);
        }

        public void UpdateDb(ForexCurrencyRateModel live)
        {
            _context.Update(live);
        }
        public void SaveToDb()
        {
            _context.SaveChanges();
        }

        public List<CurrencyModel> GetCurrencies()
        {
            return _context.Currencies.ToList();
        }
        public IEnumerable<ForexCurrencyRateModel> GetLiveCurrencies()
        {
            return _context.CurrencyExchangeRatesLive.Include(c => c.FromCurrency).Include(c => c.ToCurrency);
        }

        public IEnumerable<HistoryRateModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {

            DateTime now = DateTime.Now;

            //İstek Gece Saatlerinde Atıldıysa, İstenen Currency için Önceki Günün Tüm History Değerlerini Getir
            if (now.Hour >= 0 && now.Hour < 9)
            {
                DateTime lastInsertDate = new DateTime(now.Year, now.Month, now.Day - 1, 9, 0, 0);
                return _context.CurrencyExchangeRatesHistory
                                                            .Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId) && (c.ToCurrency.CurrencyModelId == toCurrencyModelId) && (c.LastRefreshedDate.CompareTo(lastInsertDate) >= 0))
                                                            .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            return _context.CurrencyExchangeRatesHistory
                                                        .Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId) && (c.ToCurrency.CurrencyModelId == toCurrencyModelId) && (c.LastRefreshedDate.Day == now.Day))
                                                        .Include(c => c.FromCurrency).Include(c => c.ToCurrency);
        }


    }
}
