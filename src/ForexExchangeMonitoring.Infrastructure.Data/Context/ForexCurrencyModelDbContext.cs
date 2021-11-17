using System;
using ForexExchangeMonitoring.Domain.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ForexExchangeMonitoring.Infrastructure.Data
{
    public class ForexCurrencyModelDbContext : DbContext
    {
        public ForexCurrencyModelDbContext(DbContextOptions<ForexCurrencyModelDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForexCurrencyRateModel>().Property(p => p.FromCurrencyId).HasColumnName("from_currency_id");
            modelBuilder.Entity<ForexCurrencyRateModel>().Property(p => p.ToCurrencyId).HasColumnName("to_currency_id");

            modelBuilder.Entity<HistoryRateModel>().Property(p => p.FromCurrencyId).HasColumnName("from_currency_id");
            modelBuilder.Entity<HistoryRateModel>().Property(p => p.ToCurrencyId).HasColumnName("to_currency_id");

            modelBuilder.Entity<ForexCurrencyRateModel>().HasOne(m => m.FromCurrency)
                                        .WithMany(m => m.FromForexCurrencyModels).HasForeignKey(m => m.FromCurrencyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ForexCurrencyRateModel>().HasOne(m => m.ToCurrency)
                                        .WithMany(m => m.ToForexCurrencyModels).HasForeignKey(m => m.ToCurrencyId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForexCurrencyRateModel>().HasMany(m => m.Histories).WithOne(m => m.ForexCurrencyModel);
        }

        public DbSet<ForexCurrencyRateModel> CurrencyExchangeRatesLive { get; set; }
        public DbSet<CurrencyModel> Currencies { get; set; }
        public DbSet<HistoryRateModel> CurrencyExchangeRatesHistory { get; set; }
    }
}
