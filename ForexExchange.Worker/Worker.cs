using ForexExchange.Worker.Interface;
using ForexExchange.Worker.Servicce;
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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        ForexCurrencyModelDbContext _dbContextt = new ForexCurrencyModelDbContext();
        public Worker(ILogger<Worker> logger/*, WorkerService workerService*//*, ForexCurrencyModelDbContext dbContext*/)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //_workerService.TakeDataFromApi();

                string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=JPY&apikey=1QHQEJN8ATXPVNI4";
                Uri queryUri = new Uri(QUERY_URL);
                using (WebClient client = new WebClient())
                {
                    dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));

                    ForexCurrencyModel model;
                    foreach (var item in json_data.Values)
                    {
                        model = new ForexCurrencyModel
                        {
                            FromCurrencyCode = item.GetProperty("1. From_Currency Code").ToString(),
                            ToCurrencyCode = item.GetProperty("3. To_Currency Code").ToString(),
                            ExchangeRate = item.GetProperty("5. Exchange Rate").ToString(),
                            LastRefreshedDate = Convert.ToDateTime(item.GetProperty("6. Last Refreshed").ToString())
                        };
                        _dbContextt.RealTimeCurrencyExchangeRates.Add(model);
                        _dbContextt.SaveChanges();
                    }
                }
               
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
