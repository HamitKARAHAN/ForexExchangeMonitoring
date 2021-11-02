using ForexExchangeMonitoring.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForexExchange.Controllers
{
    public class CurrencyController : Controller
    {
        private ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        // GET: ProjectController
        public ActionResult Index()
        {
            return View(_currencyService.GetCurrencies());
        }
        // GET: ProjectController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
