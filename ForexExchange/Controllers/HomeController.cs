using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Domain.Models;
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
namespace ForexExchange.Controllers
{
    public class HomeController : Controller
    {
        private ICurrencyService _currencyService;

        public HomeController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public ActionResult Index()
        {
            DateTime now = DateTime.Now;
            return View(_currencyService.GetCurrencies(now));
        }
        // GET: HomeController/History/id
        public ActionResult History(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return View(_currencyService.getCurrencyRate(fromCurrencyModelId, toCurrencyModelId));
        }
    }
}
