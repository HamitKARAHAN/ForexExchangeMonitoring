using System;
using ForexExchangeMonitoring.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ForexExchangeMonitoring.Infrastructure.Data
{
    public class ForexCurrencyModelDbContext : DbContext
    {
        public ForexCurrencyModelDbContext(DbContextOptions<ForexCurrencyModelDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForexCurrencyModel>()
             .Property(p => p.FromCurrencyId)
             .HasColumnName("from_currency_id");
            modelBuilder.Entity<ForexCurrencyModel>()
             .Property(p => p.ToCurrencyId)
             .HasColumnName("to_currency_id");



            modelBuilder.Entity<ForexCurrencyModel>().HasOne<CurrencyModel>(s => s.FromCurrency).WithMany(g => g.FromCurrencyIds)
                .HasForeignKey(s => s.FromCurrencyId).OnDelete(DeleteBehavior.ClientCascade); 

            modelBuilder.Entity<ForexCurrencyModel>().HasOne<CurrencyModel>(s => s.ToCurrency).WithMany(g => g.ToCurrencyIds)
                .HasForeignKey(s => s.ToCurrencyId).OnDelete(DeleteBehavior.ClientCascade); 
        }

        public DbSet<ForexCurrencyModel> RealTimeCurrencyExchangeRates { get; set; }
        public DbSet<CurrencyModel> Currencies { get; set; }
    }
}
