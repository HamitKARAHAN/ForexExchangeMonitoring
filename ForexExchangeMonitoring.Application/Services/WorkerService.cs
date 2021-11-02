using ForexExchangeMonitoring.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using ForexExchangeMonitoring.Domain.Interfaces;
using ForexExchangeMonitoring.Domain.Models;

namespace ForexExchangeMonitoring.Application.Services
{
    public class WorkerService : IWorkerService
    {
        public ICurrencyRepository _currencyRepository;
        public WorkerService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public void TakeDataFromApi()
        {
            string[] currencies = { "USD", "TRY", "EUR", "GBP", "JPY", "CHF", "KWD", "RUB" };

            string QUERY_URL = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=JPY&apikey=1QHQEJN8ATXPVNI4";
            Uri queryUri = new Uri(QUERY_URL);
            using (WebClient client = new WebClient())
            {
                dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));

                ForexCurrencyModel model;
                foreach (var item in json_data.Values)
                {
                    model = new ForexCurrencyModel
                    {
                        FromCurrencyCode = item.GetProperty("1. From_Currency Code").ToString(),
                        ToCurrencyCode = item.GetProperty("3. To_Currency Code").ToString(),
                        ExchangeRate = item.GetProperty("5. Exchange Rate").ToString(),
                        LastRefreshedDate = Convert.ToDateTime(item.GetProperty("6. Last Refreshed").ToString())
                    };
                    _currencyRepository.WriteToDb(model);
                }   
            }
        }
    }
}
