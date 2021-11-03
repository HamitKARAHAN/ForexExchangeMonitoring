using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.Services;
using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ForexExchangeMonitoring.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        }
    }
}
