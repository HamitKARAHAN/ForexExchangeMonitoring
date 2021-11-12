using ForexExchangeMonitoring.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ForexExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public HomeController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        // GET: HomeController/Index/
        public ActionResult Index()
        {
         
            return View(_currencyService.GetLiveCurrencies());
        }

        // GET: HomeController/History/{id1, id2}
        public ActionResult History(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return View(_currencyService.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId));
        }
    }
}
