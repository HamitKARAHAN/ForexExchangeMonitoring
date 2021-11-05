using ForexExchangeMonitoring.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   new CurrencyModel{CurrencyName="USD"},
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
