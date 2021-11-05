
using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.Services;
using ForexExchangeMonitoring.Infrastructure.Data;
using ForexExchangeMonitoring.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ForexExchange.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            DataSeed.CreateDbIfNotExists(host);
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {    
                    services.AddHostedService<Worker>();
                    services.AddDbContext<ForexCurrencyModelDbContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=ForexEchangeMonitoring;Trusted_Connection=True;MultipleActiveResultSets=true"));
                    
                });
    }
}
