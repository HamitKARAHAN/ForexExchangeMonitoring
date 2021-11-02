using ForexExchangeMonitoring.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private IWorkerService _workerService;

        public Worker(ILogger<Worker> logger/*, IWorkerService worker*/)
        {
            _logger = logger;
            //_workerService = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //_workerService.TakeDataFromApi();
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
