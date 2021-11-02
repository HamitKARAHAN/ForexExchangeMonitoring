
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForexExchange.Controllers
{
    public class HomeController : Controller
    {

        private readonly ForexCurrencyModelDbContext _dbContext;

        public HomeController(ForexCurrencyModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public IActionResult Index()
        {
            string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=JPY&apikey=1QHQEJN8ATXPVNI4";
            Uri queryUri = new Uri(QUERY_URL);
            using (WebClient client = new WebClient())
            {
                dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));

                //RealTimeCurrencyExchangeRate rt;
                //foreach (var item in json_data.Values)
                //{
                //    rt = new RealTimeCurrencyExchangeRate
                //    {
                //        FromCurrencyCode = item.GetProperty("1. From_Currency Code").ToString(),
                //        ToCurrencyCode = item.GetProperty("3. To_Currency Code").ToString(),
                //        LastRefreshedDate = Convert.ToDateTime(item.GetProperty("6. Last Refreshed").ToString())
                //    };

                //    _dbContext.RealTimeCurrencyExchangeRates.Add(rt);
                //    _dbContext.SaveChanges();
               // }
            }
            return View();
        }


    }
}
