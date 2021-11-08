using ForexExchangeMonitoring.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ForexExchangeMonitoring.Infrastructure.Data
{
    public class ForexCurrencyModelDbContext : DbContext
    {
        public ForexCurrencyModelDbContext(DbContextOptions<ForexCurrencyModelDbContext> options) : base(options)
        {
        }
        public ForexCurrencyModelDbContext() : base()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("myconn");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public DbSet<ForexCurrencyModel> RealTimeCurrencyExchangeRates { get; set; }
        public DbSet<CurrencyModel> Currencies { get; set; }
    }
}
