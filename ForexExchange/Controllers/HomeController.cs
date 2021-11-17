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
        public IActionResult Index()
        {
            return View(_currencyService.GetLiveCurrencies());
        }

        [Route("Home/Index/{sortOrder?}")]
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
            #region Searching Datas
            ViewData["FromCurrencyFilter"] = from;
            ViewData["ToCurrencyFilter"] = to;
            ViewData["RateCurrencyFilter"] = minRate;
            #endregion
            return View("Index", _currencyService.GetLiveCurrenciesBySearch(from, to, minRate));
        }


        // GET: HomeController/History/{id1, id2}
        public ActionResult History(int fromCurrencyModelId, int toCurrencyModelId)
        {
            return View(_currencyService.GetCurrencyHistory(fromCurrencyModelId, toCurrencyModelId));
        }
    }
}
