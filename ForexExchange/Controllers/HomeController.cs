﻿using ForexExchangeMonitoring.Application.Interfaces;
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
            return View(_currencyService.GetCurrencies());
        }
        // GET: ProjectController/History/5
        public ActionResult History(int _currencyModelId)
        {
            return View();
        }

        
    }
}
