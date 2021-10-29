using ForexExchange.Models;
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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=JPY&apikey=1QHQEJN8ATXPVNI4";
            Uri queryUri = new Uri(QUERY_URL);
            Dictionary<string, object> model1=new Dictionary<string, object>();
            using (WebClient client = new WebClient())
            {
                dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
                model1 = json_data;
                

                var nameOfProperty = "1. From_Currency Code";




                foreach (var item in model1)
                {
                    var propertyInfo = item.Value.GetType().GetProperty(nameOfProperty);
                    var value = propertyInfo.GetValue(item.Value, null);
                }
            }
            return View(model1);
        }


    }
}
