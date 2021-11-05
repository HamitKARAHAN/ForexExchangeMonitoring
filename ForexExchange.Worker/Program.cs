
using ForexExchangeMonitoring.Infrastructure.Data;
using ForexExchangeMonitoring.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForexExchange.Worker
{
    public class Program
    {

        public IConfiguration Configuration { get; }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ForexCurrencyModelDbContext>(options =>
                    {
                        options.UseSqlServer(
                        "Server=localhost;Database=ForexEchangeMonitoring;Trusted_Connection=True;MultipleActiveResultSets=true");
                    });
                    services.AddHostedService<Worker>();
                });

        public void Configure(IApplicationBuilder app)
        {
            DataSeeding.Seed(app);
        }
    }
}
