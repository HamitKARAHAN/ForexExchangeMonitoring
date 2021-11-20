using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.ViewModels;
using ForexExchangeMonitoring.Domain.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IDistributedCache _distributedCache;
        public HomeController(ICurrencyService currencyService, IDistributedCache distributedCache)
        {
            _currencyService = currencyService;
            _distributedCache = distributedCache;
        }

        public IActionResult Index(string sortOrder)
        {
            var cacheKey = "currencies";
            LiveCurrenciesRateViewModel m1 = new LiveCurrenciesRateViewModel();
            var redisCurrenciesList = _distributedCache.Get(cacheKey);
            string currencies;
            if (redisCurrenciesList != null)
            {
                currencies = Encoding.UTF8.GetString(redisCurrenciesList);
                m1.ForexLiveCurrencies = JsonConvert.DeserializeObject<IEnumerable<ForexCurrencyRateModel>>(currencies,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            }
            else
            {
                m1 = _currencyService.GetLiveCurrenciesBySort(sortOrder);


                currencies = JsonConvert.SerializeObject(m1.ForexLiveCurrencies, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

                redisCurrenciesList = Encoding.UTF8.GetBytes(currencies);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));                            
                _distributedCache.Set(cacheKey, redisCurrenciesList, options);


            }

            #region Sorting Datas
            ViewData["FromSortParam"] = sortOrder == "from_asc" ? "from_desc" : "from_asc";
            ViewData["ToSortParam"] = sortOrder == "to_asc" ? "to_desc" : "to_asc";
            ViewData["RateSortParam"] = sortOrder == "rate_asc" ? "rate_desc" : "rate_asc";
            #endregion
            return View(m1);
        }

        public IActionResult IndexSearch(string from, string to, string minRate)
        {
            return View("Index", _currencyService.GetLiveCurrenciesBySearch(from, to, minRate));
        }


        // GET: HomeController/History/{id1, id2}
        public ActionResult History(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return View(_currencyService.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId));
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var cacheKey = "weatherList";
        //    string serializedCustomerList;
        //    List<string> weatherList = new List<string>();
        //    var redisCustomerList = _distributedCache.Get(cacheKey);
        //    if (redisCustomerList != null)
        //    {
        //        serializedCustomerList = Encoding.UTF8.GetString(redisCustomerList);
        //        weatherList = JsonConvert.DeserializeObject<List<string>>(serializedCustomerList);
        //    }
        //    else
        //    {
        //        weatherList.Add("dog");
        //        weatherList.Add("cat");
        //        serializedCustomerList = JsonConvert.SerializeObject(weatherList);
        //        redisCustomerList = Encoding.UTF8.GetBytes(serializedCustomerList);
        //        var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
        //        _distributedCache.Set(cacheKey, redisCustomerList, options);
        //    }
        //    return Ok(weatherList);
        //}
    }
}
