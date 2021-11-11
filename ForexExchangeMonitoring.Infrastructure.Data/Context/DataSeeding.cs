using ForexExchangeMonitoring.Domain.DbModels;
using System.Linq;

namespace ForexExchangeMonitoring.Infrastructure.Data.Context
{
    public static class DataSeeding
    {
        public static void Seed(ForexCurrencyModelDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Currencies.Any())
                return;       // DB has been seeded
            else
            {
                var currencies = new CurrencyModel[]
                {
                   new CurrencyModel{CurrencyName ="USD"},
                   new CurrencyModel{CurrencyName ="TRY"},
                   new CurrencyModel{CurrencyName ="EUR"},
                   new CurrencyModel{CurrencyName ="GBP"},
                   new CurrencyModel{CurrencyName ="JPY"},
                   new CurrencyModel{CurrencyName ="CHF"},
                   new CurrencyModel{CurrencyName ="KWD"},
                   new CurrencyModel{CurrencyName ="RUB"}
                };
                foreach (var currency in currencies)
                {
                    context.Currencies.Add(currency);
                }
                context.SaveChanges();
            }
        }
    }
}
