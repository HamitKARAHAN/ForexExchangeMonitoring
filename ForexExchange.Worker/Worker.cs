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
using System.IO;

namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
     
            _logger = logger;
            _serviceProvider = serviceProvider;

        }
        private readonly ILogger<Worker> _logger;
        public IServiceProvider _serviceProvider;



        //private string ConfigureApiKeys(int i)
        //{

        //    string currentApiKey = Configuration.GetValue<string>("ApiKeys:ApiKey"+i);

        //    return currentApiKey;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                string[] currencies = { "USD", "TRY", "EUR", "GBP", "JPY", "CHF", "KWD", "RUB" };
                //string apiKey = "P1JJN2Q8YP2HQXUE";
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {

                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ForexCurrencyModelDbContext>();
                        await Get(context, currencies);
                    }
                    await Task.Delay(1000, stoppingToken);

                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        private async Task Get(ForexCurrencyModelDbContext _dbContextt, string[] currencies)
        {
            for (int i = 0; i < currencies.Length; i++)
            {
                int keyCounter = 0;
                //string apiKey = ConfigureApiKeys(i);

                for (int j = 0; j < currencies.Length; j++)
                {
                    if (i.Equals(j))
                        continue;

                    keyCounter++;
                    string apikeyy = "P1JJN2Q8YP2HQXUE";

                    string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="
                            + currencies[i] + "&to_currency=" + currencies[j] + "&apikey=" + apikeyy;

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
