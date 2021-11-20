using ForexExchangeMonitoring.Application.Interfaces;
using ForexExchangeMonitoring.Application.ViewModels;
using ForexExchangeMonitoring.Domain.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrencyService _currencyService;
        public HomeController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public IActionResult Index(string sortOrder)
        {
            #region Sorting Datas
            ViewData["FromSortParam"] = sortOrder == "from_asc" ? "from_desc" : "from_asc";
            ViewData["ToSortParam"] = sortOrder == "to_asc" ? "to_desc" : "to_asc";
            ViewData["RateSortParam"] = sortOrder == "rate_asc" ? "rate_desc" : "rate_asc";
            #endregion
            return View(_currencyService.GetLiveCurrenciesBySort(sortOrder));
        }

        public IActionResult IndexSearch(string from, string to, string minRate)
        {
            return View("Index", _currencyService.GetLiveCurrenciesBySearch(from, to, minRate));
        }


        // GET: HomeController/History/{id1, id2}
        public ActionResult History(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return View(_currencyService.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId));
        }
    }
}
