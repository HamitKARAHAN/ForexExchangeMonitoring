using ForexExchangeMonitoring.Domain.DbModels;
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
using Vonage.Request;
using Vonage.Messaging;
using ForexExchange.Worker.Helpers;
using Log;

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
            DateTime now = DateTime.Now;                    //Þuan
            DateTime todayMidnight = DateTime.Today;        //Bugün saat 00:00
            int dayToday = (int)now.DayOfWeek;              //Haftanýn kaçýncý günü?
            TimeSpan nineHour = TimeSpan.FromHours(9);      //9 saatlik süre

            int waitingTime;
            //Eger Hafta Ýçi Herhangi Bir Gün Ýse
            if (dayToday < 6)
            {
                if (now.Hour < 9)
                {
                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Warning, Message = "TimeDelay", MessageDetail = "Worker Will Wait Until " + (todayMidnight + nineHour) });
                    //(Þimdi - (Bugün saat sabah 9'u getirir))
                    waitingTime = (int)(Math.Abs((now - (todayMidnight + nineHour)).TotalMilliseconds));
                    await Task.Delay(waitingTime);
                    return;
                }
                else if (now.Hour >= 18 && now.Minute >= 5)
                {
                    //Eðer Cuma Akþamý ise Pazartesi Saat Sabah 9'a Kadar Bekle
                    if (dayToday == 5)
                    {
                        LogHelper.Log(new LogModel { EventType = Enums.LogType.Warning, Message = "TimeDelay", MessageDetail = "Worker Will Wait Until " + (todayMidnight.AddDays(3) + nineHour) });
                        //(Þimdi - (Pazartesi saat sabah 9'u getirir))
                        waitingTime = (int)(Math.Abs((now - (todayMidnight.AddDays(3) + nineHour)).TotalMilliseconds));
                        await Task.Delay(waitingTime);
                        return;
                    }

                    //Eðer Hafta içi ise Bir Sonraki Gün Saat Sabah 9'a Kadar Bekle
                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Warning, Message = "TimeDelay", MessageDetail = "Worker Will Wait Until " + (todayMidnight.AddDays(1) + nineHour) });
                    waitingTime = (int)(Math.Abs((now - (todayMidnight.AddDays(1) + nineHour)).TotalMilliseconds));
                    await Task.Delay(waitingTime);
                    return;
                }

                if (now.Minute < 30 && now.Minute != 0)
                {
                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Warning, Message = "TimeDelay", MessageDetail = "Worker Will Wait Until " + (todayMidnight.AddHours(now.Hour).AddMinutes(30)) });
                    waitingTime = (int)(Math.Abs((now - todayMidnight.AddHours(now.Hour).AddMinutes(30)).TotalMilliseconds));
                    await Task.Delay(waitingTime);
                }
                else if (now.Minute > 30)
                {
                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Warning, Message = "TimeDelay", MessageDetail = "Worker Will Wait Until " + (todayMidnight.AddHours(now.Hour + 1)) });
                    waitingTime = (int)(Math.Abs((now - todayMidnight.AddHours(now.Hour + 1)).TotalMilliseconds));
                    await Task.Delay(waitingTime);
                }
            }
            else
            {
                //Eðer Hafta Sonu Herhangi Bir Gün ise Gelecek Pazartesiye kadar bekle
                int daysUntilNextMonday = ((int)DayOfWeek.Monday - dayToday + 7) % 7;
                LogHelper.Log(new LogModel { EventType = Enums.LogType.Warning, Message = "TimeDelay", MessageDetail = "Worker Will Wait Until " + (todayMidnight.AddDays(daysUntilNextMonday) + nineHour) });
                waitingTime = (int)(Math.Abs((now - (todayMidnight.AddDays(daysUntilNextMonday) + nineHour)).TotalMilliseconds));
                await Task.Delay(waitingTime);
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string QUERY_URL = _configuration.GetConnectionString("QUERY_URL");
            bool isFirstRunning = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                await TimeDelay();
                _logger.LogInformation("Worker running at   ===========>   {time}", DateTime.Now);
                try
                {

                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Info, Message = "ExecuteAsync", MessageDetail = "Worker running at   ===========> " + DateTime.Now });
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var currencyRepository = scope.ServiceProvider.GetService<ICurrencyRepository>();
                        await GetForexDatas(QUERY_URL, currencyRepository, isFirstRunning);
                    }
                    await Task.Delay(1800000, stoppingToken);
                }
                catch (Exception ex)
                {
                    #region -- Send Message to Phone
                    var VONAGE_API_KEY = _configuration.GetConnectionString("Vonage-ApiKey");
                    var VONAGE_API_SECRET = _configuration.GetConnectionString("Vonage-ApiSecret");
                    var credentials = Credentials.FromApiKeyAndSecret(VONAGE_API_KEY, VONAGE_API_SECRET);
                    var client = new SmsClient(credentials);
                    var request = new SendSmsRequest
                    {
                        To = _configuration.GetConnectionString("Vonage-PhoneNumber"),
                        From = _configuration.GetConnectionString("Vonage-PhoneNumber"),
                        Text = "FOREX EXCHANGE MONITORING CRASHED !! PLEASE RESTART AGAIN !!"
                    };
                    var response = client.SendAnSms(request);
                    #endregion
                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Message = "ExecuteAsync", Exception = ex, MessageDetail = "Worker Crashed. A SMS Sent to : " + response.Messages[0].To + " Remaining Api Bund € : " + response.Messages[0].RemainingBalance });
                    return;
                }
            }
        }

        private async Task GetForexDatas(string QUERY_URL, ICurrencyRepository _currencyRepository, bool isFirstRunning)
        {
            using (var _client = new HttpClient())
            {
                try
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
                catch (Exception ex)
                {
                    LogHelper.Log(new LogModel { EventType = Enums.LogType.Error, Message = "GetForexDatas", Exception = ex, MessageDetail = "Api is Not giving Response " });
                }
            }
        }

        private void SaveForexDatas(ICurrencyRepository _currencyRepository, Root jsonModel, List<CurrencyModel> currenciesFromDb, List<ForexCurrencyRateModel> liveCurrenciesFromDb)
        {
            foreach (var item in jsonModel.Quotes)
            {
                var liveData = liveCurrenciesFromDb
                    .FirstOrDefault(c => (c.FromCurrency.CurrencyName == item.BaseCurrency) && (c.ToCurrency.CurrencyName == item.QuoteCurrency));

                //RealTime Tablosunundaki sadece geerekli alanlarý güncelle
                liveData.ExchangeRate = item.Ask;
                liveData.LastRefreshedDate = jsonModel.RequestedTime.ToLocalTime();
                _currencyRepository.UpdateDb(liveData);

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
