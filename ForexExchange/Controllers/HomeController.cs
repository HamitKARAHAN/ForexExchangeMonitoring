using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Domain.Models;
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
namespace ForexExchange.Controllers
{
    public class HomeController : Controller
    {
        private ICurrencyService _currencyService;
        private readonly ForexCurrencyModelDbContext _dbContext;

        public HomeController(ForexCurrencyModelDbContext dbContext, ICurrencyService currencyService)
        {
            _dbContext = dbContext;
            _currencyService = currencyService;
        }

        public IActionResult Index()
        {
            //string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=JPY&apikey=1QHQEJN8ATXPVNI4";
            //Uri queryUri = new Uri(QUERY_URL);
            //using (WebClient client = new WebClient())
            //{
            //    dynamic json_data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
            //    ForexCurrencyModel rt;
            //    foreach (var item in json_data.Values)
            //    {
            //        rt = new ForexCurrencyModel
            //        {
            //            FromCurrencyCode = item.GetProperty("1. From_Currency Code").ToString(),
            //            ToCurrencyCode = item.GetProperty("3. To_Currency Code").ToString(),
            //            ExchangeRate = item.GetProperty("5. Exchange Rate").ToString(),
            //            LastRefreshedDate = Convert.ToDateTime(item.GetProperty("6. Last Refreshed").ToString())
            //        };
            //        _dbContext.RealTimeCurrencyExchangeRates.Add(rt);
            //        _dbContext.SaveChanges();
            //    }
            //}
            return View();
        }


    }
}
