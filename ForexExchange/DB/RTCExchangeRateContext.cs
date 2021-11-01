using ForexExchange.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForexExchange.DB
{
    public class RTCExchangeRateContext : DbContext
    {
        public RTCExchangeRateContext(DbContextOptions options) : base (options)
        {
        }
        public DbSet<RealTimeCurrencyExchangeRate> RealTimeCurrencyExchangeRates { get; set; }
    }
}
