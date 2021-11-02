using ForexExchangeMonitoring.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ForexExchangeMonitoring.Infrastructure.Data
{
    public class ForexCurrencyModelDbContext : DbContext
    {
        public ForexCurrencyModelDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ForexCurrencyModel> RealTimeCurrencyExchangeRates { get; set; }
    }
}
