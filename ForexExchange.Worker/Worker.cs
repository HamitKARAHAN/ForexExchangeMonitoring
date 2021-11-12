using ForexExchangeMonitoring.Domain.DbModels;
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using ForexExchangeMonitoring.Domain.JsonModels;
using Microsoft.EntityFrameworkCore;
using ForexExchangeMonitoring.Domain.Interfaces;

namespace ForexExchange.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        private async Task TimeDelay()
        {
            DateTime now = DateTime.Now;
            if (now.Hour < 9)
            {
                int waitingTime = (int)(Math.Abs((now - (new DateTime(now.Year, now.Month, now.Day, 9, 0, 0))).TotalMilliseconds));
                await Task.Delay(waitingTime);
            }
            else if (now.Hour >= 18 && now.Minute >= 10)
            {
                int waitingTime = (int)(Math.Abs((now - (new DateTime(now.Year, now.Month, now.Day + 1, 9, 0, 0))).TotalMilliseconds));
                await Task.Delay(waitingTime);
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool isFirstRunning = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                await TimeDelay();
                _logger.LogInformation("Worker running at:   {time}", DateTime.Now);
                try
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var currencyRepository = scope.ServiceProvider.GetService<ICurrencyRepository>();
                        await GetForexDatas(currencyRepository, isFirstRunning);
                    }
                    await Task.Delay(1800000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while inserting to DB");
                    return;
                }
            }
        }

        private async Task GetForexDatas(ICurrencyRepository _currencyRepository, bool isFirstRunning)
        {
            string QUERY_URL = _configuration.GetConnectionString("QUERY_URL");
            using (var _client = new HttpClient())
            {
                var response = await _client.GetAsync(QUERY_URL);
                string responseBody = await response.Content.ReadAsStringAsync();
                Root jsonModel = JsonConvert.DeserializeObject<Root>(responseBody);

                var currenciesFromDb = _currencyRepository.GetCurrencies().ToList();
                var liveCurrenciesFromDb = _currencyRepository.GetLiveCurrencies().ToList();

                //Program ilk defa çalýþýyorsa
                if (isFirstRunning)
                {
                    SaveForexDatasFirstTime(_currencyRepository, jsonModel, currenciesFromDb, liveCurrenciesFromDb);
                    isFirstRunning = false;
                }
                else
                    SaveForexDatas(_currencyRepository, jsonModel, currenciesFromDb, liveCurrenciesFromDb);
            }

            {
                //#region   --Bu kod bloðu sadece bir kez, program baþlatýldýðýnda çalýþacak--
                //if (isFirstRunning)
                //{
                //    foreach (var item in jobject.Quotes)
                //    {
                //        var entity = liveCurrenciesFromDb
                //                .FirstOrDefault(c => (c.FromCurrency.CurrencyName == item.BaseCurrency) && (c.ToCurrency.CurrencyName == item.QuoteCurrency));
                //        ForexCurrencyRateModel live = new();
                //        if (entity == null)
                //        {
                //            live.FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId;
                //            live.ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId;
                //            live.FromCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency);
                //            live.ToCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency);
                //            live.ExchangeRate = item.Ask;
                //            live.LastRefreshedDate = jobject.RequestedTime.ToLocalTime();

                //            //RealTime Tablosuna Ekle
                //            _currencyRepository.AddLiveExchangeRate(live);
                //        }
                //        else
                //        {
                //            HistoryRateModel history = new()
                //            {
                //                FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId,
                //                ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId,
                //                FromCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency),
                //                ToCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency),
                //                ExchangeRate = item.Ask,
                //                LastRefreshedDate = jobject.RequestedTime.ToLocalTime(),
                //                ForexCurrencyModel = entity ?? live
                //            };
                //            //History Tablosuna Ekle
                //            _currencyRepository.AddHistoryExchangeRate(history);

                //            //RealTime Tablosunundaki sadece geerekli alanlarý güncelle
                //            entity.ExchangeRate = item.Ask;
                //            entity.LastRefreshedDate = jobject.RequestedTime.ToLocalTime();
                //            _currencyRepository.UpdateDb(entity);
                //        }

                //    }
                //    //sayacý artýralým ve bu döngüye bir daha asla girmesin
                //    isFirstRunning = false;
                //    //Ýki Tabloyu da Kaydet
                //    _currencyRepository.SaveToDb();
                //    return;
                //}
                //#endregion

                //foreach (var item in jobject.Quotes)
                //{
                //    var entity = liveCurrenciesFromDb
                //        .FirstOrDefault(c => (c.FromCurrency.CurrencyName == item.BaseCurrency) && (c.ToCurrency.CurrencyName == item.QuoteCurrency));

                //    HistoryRateModel history = new()
                //    {
                //        FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId,
                //        ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId,
                //        FromCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency),
                //        ToCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency),
                //        ExchangeRate = item.Ask,
                //        LastRefreshedDate = jobject.RequestedTime.ToLocalTime(),
                //        ForexCurrencyModel = entity
                //    };
                //    //History Tablosuna ekle
                //    _currencyRepository.AddHistoryExchangeRate(history);

                //    //RealTime Tablosunundaki sadece geerekli alanlarý güncelle
                //    entity.ExchangeRate = item.Ask;
                //    entity.LastRefreshedDate = jobject.RequestedTime.ToLocalTime();
                //    _currencyRepository.UpdateDb(entity);
                //}
                ////Ýki tabloyu da Kaydet
                //_currencyRepository.SaveToDb();
            }
        }

        private void SaveForexDatas(ICurrencyRepository _currencyRepository, Root jsonModel, List<CurrencyModel> currenciesFromDb, List<ForexCurrencyRateModel> liveCurrenciesFromDb)
        {
            foreach (var item in jsonModel.Quotes)
            {
                var liveData = liveCurrenciesFromDb
                    .FirstOrDefault(c => (c.FromCurrency.CurrencyName == item.BaseCurrency) && (c.ToCurrency.CurrencyName == item.QuoteCurrency));
                HistoryRateModel history = new()
                {
                    FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId,
                    ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId,
                    FromCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency),
                    ToCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency),
                    ExchangeRate = item.Ask,
                    LastRefreshedDate = jsonModel.RequestedTime.ToLocalTime(),
                    ForexCurrencyModel = liveData
                };
                //History Tablosuna ekle
                _currencyRepository.AddHistoryExchangeRate(history);

                //RealTime Tablosunundaki sadece geerekli alanlarý güncelle
                liveData.ExchangeRate = item.Ask;
                liveData.LastRefreshedDate = jsonModel.RequestedTime.ToLocalTime();
                _currencyRepository.UpdateDb(liveData);
            }
            //Ýki tabloyu da Kaydet
            _currencyRepository.SaveToDb();
        }

        private void SaveForexDatasFirstTime(ICurrencyRepository _currencyRepository, Root jsonModel, List<CurrencyModel> currenciesFromDb, List<ForexCurrencyRateModel> liveCurrenciesFromDb)
        {
            foreach (var item in jsonModel.Quotes)
            {
                var liveData = liveCurrenciesFromDb
                        .FirstOrDefault(c => (c.FromCurrency.CurrencyName == item.BaseCurrency) && (c.ToCurrency.CurrencyName == item.QuoteCurrency));
                ForexCurrencyRateModel live = new();
                if (liveData == null)
                {
                    live.FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId;
                    live.ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId;
                    live.FromCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency);
                    live.ToCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency);
                    live.ExchangeRate = item.Ask;
                    live.LastRefreshedDate = jsonModel.RequestedTime.ToLocalTime();

                    //RealTime Tablosuna Ekle
                    _currencyRepository.AddLiveExchangeRate(live);
                }
                else
                {
                    //RealTime Tablosunundaki sadece geerekli alanlarý güncelle
                    liveData.ExchangeRate = item.Ask;
                    liveData.LastRefreshedDate = jsonModel.RequestedTime.ToLocalTime();
                    _currencyRepository.UpdateDb(liveData);
                }
                HistoryRateModel history = new()
                {
                    FromCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency).CurrencyModelId,
                    ToCurrencyId = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency).CurrencyModelId,
                    FromCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.BaseCurrency),
                    ToCurrency = currenciesFromDb.FirstOrDefault(c => c.CurrencyName == item.QuoteCurrency),
                    ExchangeRate = item.Ask,
                    LastRefreshedDate = jsonModel.RequestedTime.ToLocalTime(),
                    ForexCurrencyModel = liveData ?? live
                };
                //History Tablosuna Ekle
                _currencyRepository.AddHistoryExchangeRate(history);
            }
            //Ýki tabloyu da Kaydet
            _currencyRepository.SaveToDb();
        }
    }
}
