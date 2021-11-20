using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Infrastructure.Data;
using ForexExchangeMonitoring.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
                    services.AddScoped<ICurrencyRepository, CurrencyRepository>();
                    services.AddDbContext<ForexCurrencyModelDbContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("myconn")));
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = hostContext.Configuration.GetConnectionString("redis");
                    });

                });
    }
}
