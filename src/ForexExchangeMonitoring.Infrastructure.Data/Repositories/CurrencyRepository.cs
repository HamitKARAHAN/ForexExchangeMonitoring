using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;
using ForexExchangeMonitoring.Infrastructure.Data.Helpers;
using Log;

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
            try
            {
                _context.SaveChanges();
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Info, Message = "SaveToDb", MessageDetail = "Database Has Been Changed" });
            }
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Exception = ex, Message = "SaveToDb", MessageDetail = "Couldnt Save to Database" });
            }
        }

        public IEnumerable<CurrencyModel> GetCurrencies()
        {
            try
            {
                return _context.Currencies;
            }
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Exception = ex, Message = "GetCurrencies", MessageDetail = "Couldnt get Currencies List" });
                throw new ArgumentNullException("Couldnt connect to Database");
            }
        }
        public IEnumerable<ForexCurrencyRateModel> GetLiveCurrencies()
        {
            try
            {
                return _context.CurrencyExchangeRatesLive.Include(c => c.FromCurrency).Include(c => c.ToCurrency);
            }
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Exception = ex, Message = "GetLiveCurrencies", MessageDetail = "Couldnt Get Live Data" });
                throw new ArgumentNullException("Couldnt connect to Database");
            }
        }

        public IEnumerable<ForexCurrencyRateModel> GetLiveCurrenciesBySort(string sortOrder)
        {
            var cacheKey = "liveCurrencies_" + sortOrder;
            IQueryable<ForexCurrencyRateModel> liveCurrencies = Enumerable.Empty<ForexCurrencyRateModel>().AsQueryable();
            try
            {
                var redisCurrenciesList = _distributedCache.GetString(cacheKey);
                //Aranan Veri Cache'de var ise
                if (redisCurrenciesList != null)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<ForexCurrencyRateModel>>(redisCurrenciesList,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
                }
                //Aranan Veri Cache'de Yok ise Db'den getir ve Cache'e Set le
                liveCurrencies = _context.CurrencyExchangeRatesLive.Include(c => c.FromCurrency).Include(c => c.ToCurrency).AsQueryable();
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
                if (liveCurrencies.Any())
                {
                    SetCache(liveCurrencies, cacheKey);
                }

                return liveCurrencies;
            }
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Exception = ex, Message = "Infrastructure.GetLiveCurrenciesBySort", MessageDetail = "Live Data Couldnt Found" });
            }
            return liveCurrencies;
        }
        public IEnumerable<ForexCurrencyRateModel> GetLiveCurrenciesBySearch(string from, string to, string min)
        {
            double _min;
            IQueryable<ForexCurrencyRateModel> liveCurrencies = Enumerable.Empty<ForexCurrencyRateModel>().AsQueryable();
            try
            {
                if (Double.TryParse(min, out _min)) { }
                else { _min = 0; }

                var cacheKey = "liveCurrencies" + "_from_" + (from ?? "all") + "_to_" + (to ?? "all");
                var redisCurrenciesList = _distributedCache.GetString(cacheKey);

                //Aranan Veri Cache'de var ise
                if (redisCurrenciesList != null)
                {
                    liveCurrencies = JsonConvert.DeserializeObject<IEnumerable<ForexCurrencyRateModel>>(redisCurrenciesList,
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

                liveCurrencies = _context.CurrencyExchangeRatesLive.Include(c => c.FromCurrency).Include(c => c.ToCurrency).AsQueryable();
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
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Exception = ex, Message = "Infrastructure.GetLiveCurrenciesBySearch", MessageDetail = "Live Data Couldnt Found" });
            }
            return liveCurrencies;
        }

        public IEnumerable<HistoryRateModel> GetCurrencyHistory(int fromCurrencyModelId, int toCurrencyModelId)
        {
            DateTime now = DateTime.Now;

            var cacheKey = "liveCurrencies" + "_fromCurrencyId_" + fromCurrencyModelId + "_toCurrencyId_" + toCurrencyModelId;
            IQueryable<HistoryRateModel> historyOfCurrencies = Enumerable.Empty<HistoryRateModel>().AsQueryable();
            try
            {
                var redisCurrenciesList = _distributedCache.GetString(cacheKey);
                //Aranan Veri Cache'de var ise
                if (redisCurrenciesList != null)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<HistoryRateModel>>(redisCurrenciesList,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
                }
                //Aranan Veri Cache'de Yok ise Db'den getir ve Cache'e Set le  
                if (now.Hour >= 0 && now.Hour < 9)
                {
                    //İstek Gece Saatlerinde Atıldıysa, İstenen Currency için Önceki Günün Tüm History Değerlerini Getir
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
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Exception = ex, Message = "Infrastructure.GetCurrencyHistory", MessageDetail = "History Data Couldnt Found" });
            }
            return historyOfCurrencies;
        }

        private void SetCache(IQueryable<object> currencies, string cacheKey)
        {
            try
            {
                string currenciesToString = JsonConvert.SerializeObject(currencies, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(SetCacheTime());
                _distributedCache.SetString(cacheKey, currenciesToString, options);
            }
            catch (Exception ex)
            {
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Cache, Exception = ex, Message = "SetCache", MessageDetail = "Caching Error" });
            }
        }

        private TimeSpan SetCacheTime()
        {
            DateTime now = DateTime.Now;
            if (now.Minute < 30)
                return TimeSpan.FromMinutes(Math.Abs(30 - now.Minute - 1)) + TimeSpan.FromSeconds(Math.Abs(60 - now.Second)) - TimeSpan.FromSeconds(10);
            else
                return TimeSpan.FromMinutes(Math.Abs(60 - now.Minute - 1)) + TimeSpan.FromSeconds(Math.Abs(60 - now.Second)) - TimeSpan.FromSeconds(10);
        }
    }
}
