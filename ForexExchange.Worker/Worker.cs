using ForexExchangeMonitoring.Domain.DbModels;
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
using Newtonsoft.Json.Converters;
using ForexExchangeMonitoring.Domain.JsonModels;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                await TimeDelay();
                _logger.LogInformation("Worker running at:   {time}", DateTimeOffset.Now);
                try
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ForexCurrencyModelDbContext>();
                        await WriteToDb(context);
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

        private async Task WriteToDb(ForexCurrencyModelDbContext _dbContextt)
        {
            string QUERY_URL = _configuration.GetConnectionString("QUERY_URL");
            using (var _client = new HttpClient())
            {
                var response = await _client.GetAsync(QUERY_URL);
                string responseBody = await response.Content.ReadAsStringAsync();
                Root jobject = JsonConvert.DeserializeObject<Root>(responseBody);

                var currenciesFromDb = _dbContextt.Currencies.ToList();
                foreach (var item in jobject.Quotes)
                {
                    ForexCurrencyModel model = new()
                    {
                        FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId,
                        ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId,
                        ExchangeRate = item.Ask,
                        LastRefreshedDate = jobject.RequestedTime.ToLocalTime()
                    };
                    _dbContextt.RealTimeCurrencyExchangeRates.Add(model);
                }
                _dbContextt.SaveChanges();
            }
        }
    }
}
