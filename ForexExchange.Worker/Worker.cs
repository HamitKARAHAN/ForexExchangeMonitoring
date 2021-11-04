using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Domain.Models;
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
//using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        ForexCurrencyModelDbContext _dbContextt = new ForexCurrencyModelDbContext();
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string[] currencies = { "USD", "TRY", "EUR", "GBP", "JPY", "CHF", "KWD", "RUB" };
                string apiKey = "P1JJN2Q8YP2HQXUE";
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Get(apiKey, currencies);
                {
                    
                    //for (int i = 0; i < currencies.Length; i++)
                    //{
                    //    for (int j = 0; j < currencies.Length; j++)
                    //    {
                    //        if (i.Equals(j))
                    //            continue;
                    //        string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="
                    //                + currencies[i] + "&to_currency=" + currencies[j] + "&apikey="+apiKey;
                    //        Uri queryUri = new Uri(QUERY_URL);
                    //        using (WebClient client = new WebClient())
                    //        {
                    //            Dictionary<string, ForexCurrencyModel> json_data = JsonConvert.DeserializeObject<Dictionary<string, ForexCurrencyModel>>(client.DownloadString(queryUri));
                    //            foreach (var item in json_data.Values)
                    //            {
                    //                _dbContextt.RealTimeCurrencyExchangeRates.Add(item);
                    //            }
                    //            _dbContextt.SaveChanges();

                    //        }
                    //        //string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="
                    //        //        + currencies[i] + "&to_currency=" + currencies[j] + "&apikey=P1JJN2Q8YP2HQXUE";
                    //        //Uri queryUri = new Uri(QUERY_URL);
                    //        //using (WebClient client = new WebClient())
                    //        //{
                    //        //    dynamic json_data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
                    //        //    ForexCurrencyModel rt;
                    //        //    foreach (var item in json_data.Values)
                    //        //    {
                    //        //        rt = new ForexCurrencyModel
                    //        //        {
                    //        //            FromCurrencyCode = item.GetProperty("1. From_Currency Code").ToString(),
                    //        //            ToCurrencyCode = item.GetProperty("3. To_Currency Code").ToString(),
                    //        //            ExchangeRate = item.GetProperty("5. Exchange Rate").ToString(),
                    //        //            LastRefreshedDate = Convert.ToDateTime(item.GetProperty("6. Last Refreshed").ToString())
                    //        //        };
                    //        //        _dbContextt.RealTimeCurrencyExchangeRates.Add(rt);
                    //        //        _dbContextt.SaveChanges();
                    //        //    }
                    //        //}

                    //    }
                    //}
                    //string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=JPY&apikey=1QHQEJN8ATXPVNI4";
                    //Uri queryUri = new Uri(QUERY_URL);
                    //using (WebClient client = new WebClient())
                    //{
                    //    Dictionary<string, ForexCurrencyModel> json_data = JsonConvert.DeserializeObject<Dictionary<string, ForexCurrencyModel>>(client.DownloadString(queryUri));
                    //    foreach (var item in json_data.Values)
                    //    {
                    //        _dbContextt.RealTimeCurrencyExchangeRates.Add(item);
                    //    }
                    //    _dbContextt.SaveChanges();

                    //}
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task Get(string apiKey, string[] currencies)
        {
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    if (i.Equals(j))
                        continue;
                    string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="
                            + currencies[i] + "&to_currency=" + currencies[j] + "&apikey=" + apiKey;

                    using (var _client = new HttpClient())
                    {
                        var response = await _client.GetAsync(QUERY_URL);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        Dictionary<string, ForexCurrencyModel> json_data = JsonConvert.DeserializeObject<Dictionary<string, ForexCurrencyModel>>(responseBody);
                        foreach (var item in json_data.Values)
                        {
                            _dbContextt.RealTimeCurrencyExchangeRates.Add(item);
                        }
                        _dbContextt.SaveChanges();
                    }
                }
            }
            



        }
    }
}
