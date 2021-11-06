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
using ForexExchangeMonitoring.Application.Services;
using System.Linq;
using System.Data;
namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
     
            _logger = logger;
            _serviceProvider = serviceProvider;
            Configuration = configuration;

        }
        private readonly ILogger<Worker> _logger;
        public IServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; }

        private string ConfigureApiKeys(int i)
        {

            string currentApiKey = Configuration.GetValue<string>("ApiKeys:ApiKey" + i);

            return currentApiKey;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //DateTimeOffset.Now.Hour<9 
            while (!stoppingToken.IsCancellationRequested)
            {
                string[] currencies = { "USD", "TRY", "EUR", "GBP", "JPY", "CHF", "KWD", "RUB" };
                string[] apiKeys = { "P1JJN2Q8YP2HQXUE","1QHQEJN8ATXPVNI4","WS0PV11UVNEK3QLM","O6W24FX3DFIDRDPK","UV2MT05YPS3Q6X6N",
                    "Y406MDVB2FDY21R5","N1TNQPEA1KL6HGF7","SDXAUVUO5OUG09JD", "HE6QCJGQSX3DC9LV", "OMC8V2YTJ86RZKBJ","UISJ3RHX1IJK99MJ","4K7GO9A0HTK6DVYJ"};
                //string apiKey = "P1JJN2Q8YP2HQXUE";
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ForexCurrencyModelDbContext>();
                        await Get(context, currencies,apiKeys);
                    }
                    await Task.Delay(5000, stoppingToken);

                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                    throw;
                }

            }
        }

        private async Task Get(ForexCurrencyModelDbContext _dbContextt, string[] currencies,string[] apiKeys)
        {
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    if (i.Equals(j))
                        continue;
                    string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="
                            + currencies[i] + "&to_currency=" + currencies[j] + "&apikey=" + apiKeys[j];

                    using (var _client = new HttpClient())
                    {
                        var response = await _client.GetAsync(QUERY_URL);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        Dictionary<string, ForexCurrencyModel> json_data = JsonConvert.DeserializeObject<Dictionary<string, ForexCurrencyModel>>(responseBody);
                        foreach (var item in json_data.Values)
                        {
                            item.currencyModelId = _dbContextt.Currencies.Where(c => c.CurrencyName == item.FromCurrencyCode).FirstOrDefault<CurrencyModel>();
                            _dbContextt.RealTimeCurrencyExchangeRates.Add(item);
                        }
                        _dbContextt.SaveChanges();
                    }
                }
            }
        }
    }
}
