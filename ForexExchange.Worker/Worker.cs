using ForexExchangeMonitoring.Domain.Models;
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;

namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        private string ConfigureApiKeys(int i)
        {
            string currentApiKey = _configuration.GetValue<string>("ApiKeys:ApiKey" + i);
            return currentApiKey;
        }

        private async Task TimeDelay()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            if (now.Hour < 9)
            {
                int waitingTime = (int)(Math.Abs((now - (new DateTimeOffset(now.Year, now.Month, now.Day, 9, 0, 0, TimeSpan.Zero))).TotalMilliseconds));
                await Task.Delay(waitingTime);
            }
            else if (now.Hour >= 18 && now.Minute >= 10)
            {
                int waitingTime = (int)(Math.Abs((now - (new DateTimeOffset(now.Year, now.Month, now.Day + 1, 9, 0, 0, TimeSpan.Zero))).TotalMilliseconds));
                await Task.Delay(waitingTime);
            }
            else
                return;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] currencies = { "USD", "TRY", "EUR", "GBP", "JPY", "CHF", "KWD", "RUB" };
            while (!stoppingToken.IsCancellationRequested)
            {
                await TimeDelay();
                _logger.LogInformation("Worker running at:   {time}", DateTimeOffset.Now);
                try
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ForexCurrencyModelDbContext>();
                        await WriteToDb(context, currencies);
                    }
                    await Task.Delay(1800000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while inserting to DB");
                    throw;
                }
            }
        }

        private async Task WriteToDb(ForexCurrencyModelDbContext _dbContextt, string[] currencies)
        {
            var currenciesFromDb = _dbContextt.Currencies.ToList();
            for (int i = 0; i < currencies.Length; i++)
            {
                for (int j = 0; j < currencies.Length; j++)
                {
                    if (i.Equals(j))
                        continue;
                    string currenctApiKey = ConfigureApiKeys(j);
                    string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="
                            + currencies[i] + "&to_currency=" + currencies[j] + "&apikey=" + currenctApiKey;
                    using (var _client = new HttpClient())
                    {
                        var response = await _client.GetAsync(QUERY_URL);
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Dictionary<string, ForexCurrencyModel> json_data = JsonConvert.DeserializeObject<Dictionary<string, ForexCurrencyModel>>(responseBody);

                        foreach (var item in json_data.Values)
                        {
                            item.FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.FromCurrencyCode).CurrencyModelId;
                            item.ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.ToCurrencyCode).CurrencyModelId;

                            _dbContextt.RealTimeCurrencyExchangeRates.Add(item);
                        }
                        _dbContextt.SaveChanges();
                    }
                }
            }
        }
    }
}
