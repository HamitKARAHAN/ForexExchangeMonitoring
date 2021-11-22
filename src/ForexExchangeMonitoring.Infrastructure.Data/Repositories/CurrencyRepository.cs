using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;

namespace ForexExchangeMonitoring.Infrastructure.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ForexCurrencyModelDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public CurrencyRepository(ForexCurrencyModelDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
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

        public IEnumerable<ForexCurrencyRateModel> GetLiveCurrenciesBySort(string sortOrder)
        {
            //SetCacheTime(DateTime.Now);
            var cacheKey = "liveCurrencies_" + sortOrder;
            var redisCurrenciesList = _distributedCache.Get(cacheKey);

            //Aranan Veri Cache'de var ise
            if (redisCurrenciesList != null)
            {
                string currenciesToString = Encoding.UTF8.GetString(redisCurrenciesList);
                return JsonConvert.DeserializeObject<IEnumerable<ForexCurrencyRateModel>>(currenciesToString,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            }
            //Aranan Veri Cache'de Yok ise Db'den getir ve Cache'e Set le
            else
            {
                var liveCurrencies = _context.CurrencyExchangeRatesLive.Include(c => c.FromCurrency).Include(c => c.ToCurrency).AsQueryable();
                switch (sortOrder)
                {
                    case "from_desc":
                        liveCurrencies = liveCurrencies.OrderByDescending(s => s.FromCurrency.CurrencyName);
                        break;
                    case "from_asc":
                        liveCurrencies = liveCurrencies.OrderBy(s => s.FromCurrency.CurrencyName);
                        break;
                    case "to_desc":
                        liveCurrencies = liveCurrencies.OrderByDescending(s => s.ToCurrency.CurrencyName);
                        break;
                    case "to_asc":
                        liveCurrencies = liveCurrencies.OrderBy(s => s.ToCurrency.CurrencyName);
                        break;
                    case "rate_desc":
                        liveCurrencies = liveCurrencies.OrderByDescending(s => s.ExchangeRate);
                        break;
                    case "rate_asc":
                        liveCurrencies = liveCurrencies.OrderBy(s => s.ExchangeRate);
                        break;
                }
                
                SetCache(liveCurrencies, cacheKey);
                return liveCurrencies;
            }
        }
        public IEnumerable<ForexCurrencyRateModel> GetLiveCurrenciesBySearch(string from, string to, string min)
        {
            double _min;
            if (Double.TryParse(min, out _min)) { }
            else { _min = 0; }

            var cacheKey = "liveCurrencies" + "_from_" + (from ?? "all") + "_to_" + (to ?? "all");
            var redisCurrenciesList = _distributedCache.Get(cacheKey);

            //Aranan Veri Cache'de var ise
            if (redisCurrenciesList != null)
            {
                string currenciesToString = Encoding.UTF8.GetString(redisCurrenciesList);
                var liveCurrencies = JsonConvert.DeserializeObject<IEnumerable<ForexCurrencyRateModel>>(currenciesToString,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }).AsQueryable();

                if (!String.IsNullOrEmpty(min))
                {
                    liveCurrencies = liveCurrencies.Where(s => s.ExchangeRate > _min);
                }
                return liveCurrencies;
            }
            //Aranan Veri Cache'de Yok ise Db'den getir ve Cache'e Set le
            else
            {
                var liveCurrencies = _context.CurrencyExchangeRatesLive.Include(c => c.FromCurrency).Include(c => c.ToCurrency).AsQueryable();
                if (!String.IsNullOrEmpty(from))
                {
                    liveCurrencies = liveCurrencies.Where(s => s.FromCurrency.CurrencyName == from);
                }
                if (!String.IsNullOrEmpty(to))
                {
                    liveCurrencies = liveCurrencies.Where(s => s.ToCurrency.CurrencyName == to);
                }
                if (!String.IsNullOrEmpty(min))
                {
                    liveCurrencies = liveCurrencies.Where(s => s.ExchangeRate > _min);
                }
                if (liveCurrencies.Any())
                {
                    SetCache(liveCurrencies, cacheKey);
                }
                return liveCurrencies;
            }
        }

        public IEnumerable<HistoryRateModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            DateTime now = DateTime.Now;

            var cacheKey = "liveCurrencies" + "_fromCurrencyId_" + fromCurrencyModelId + "_toCurrencyId_" + toCurrencyModelId;
            var redisCurrenciesList = _distributedCache.Get(cacheKey);

            //Aranan Veri Cache'de var ise
            if (redisCurrenciesList != null)
            {
                string currenciesToString = Encoding.UTF8.GetString(redisCurrenciesList);
                return JsonConvert.DeserializeObject<IEnumerable<HistoryRateModel>>(currenciesToString,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            }
            //Aranan Veri Cache'de Yok ise Db'den getir ve Cache'e Set le
            else
            {
                IQueryable<HistoryRateModel> historyOfCurrencies = Enumerable.Empty<HistoryRateModel>().AsQueryable();
                //İstek Gece Saatlerinde Atıldıysa, İstenen Currency için Önceki Günün Tüm History Değerlerini Getir
                if (now.Hour >= 0 && now.Hour < 9)
                {
                    DateTime lastInsertDate = new(now.Year, now.Month, now.Day - 1, 9, 0, 0);
                    historyOfCurrencies = _context.CurrencyExchangeRatesHistory
                                                                             .Include(c => c.FromCurrency).Include(c => c.ToCurrency).Include(c => c.ForexCurrencyModel)
                                                                             .Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId)
                                                                                      && (c.ToCurrency.CurrencyModelId == toCurrencyModelId)
                                                                                      && (c.LastRefreshedDate.CompareTo(lastInsertDate) >= 0));
                }
                historyOfCurrencies = _context.CurrencyExchangeRatesHistory
                                                                          .Include(c => c.FromCurrency).Include(c => c.ToCurrency).Include(c => c.ForexCurrencyModel)
                                                                          .Where(c => (c.FromCurrency.CurrencyModelId == fromCurrencyModelId)
                                                                                   && (c.ToCurrency.CurrencyModelId == toCurrencyModelId)
                                                                                   && (c.LastRefreshedDate.Day == now.Day));
                if (historyOfCurrencies.Any())
                {
                    SetCache(historyOfCurrencies, cacheKey);
                }
                return historyOfCurrencies;
            }
        }

        private void SetCache(IQueryable<object> currencies, string cacheKey)
        {     
            DateTime now = DateTime.Now;
            if (now.Minute == 30 || now.Minute == 0)
            {
                return;
            }
            string currenciesToString = JsonConvert.SerializeObject(currencies, Formatting.Indented,
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            var redisCurrenciesList = Encoding.UTF8.GetBytes(currenciesToString);
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(SetCacheTime(now));
            _distributedCache.Set(cacheKey, redisCurrenciesList, options);
        }

        private TimeSpan SetCacheTime(DateTime now)
        {
            if (now.Minute < 30)
                return TimeSpan.FromMinutes(Math.Abs(30 - DateTime.Now.Minute));
            else
                return TimeSpan.FromMinutes(Math.Abs(60 - DateTime.Now.Minute));    
        }
    }
}
