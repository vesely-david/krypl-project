using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Services.Interfaces;
using MarketDataProvider.Services;
using System.Net.Http;
using MarketDataProvider.Services.Models;
using Newtonsoft.Json;
using MarketDataProvider.Enums;

namespace MarketDataProvider.Controllers
{
    [Produces("application/json")]
    [Route("business")]
    public class BusinessController : Controller
    {
        private IMarketDataMemCacheService _marketDataService;
        private PriceService _priceService;

        public BusinessController(
            IMarketDataMemCacheService memCacheService,
            PriceService priceService)
        {
            _marketDataService = memCacheService;
            _priceService = priceService;
        }

        [HttpGet]
        [Route("price/{exchange}")]
        public IActionResult GetPrices(string exchange)
        {
            var prices = _priceService.GetExchange(exchange)?.GetValues();
            if (prices == null) return BadRequest("Not found");
            return Ok(prices);
        }


        [HttpGet]
        [Route("price")]
        public IActionResult GetAllPrices()
        {
            var prices = _priceService.GetExchange("binance")?.GetRates(); //TODO: All exchanges
            if (prices == null) return BadRequest("Not found");
            return Ok(new { binance = prices });
        }


        [HttpGet]
        [Route("rate/{exchange}")]
        public IActionResult GetRates(string exchange)
        {
            var prices = _priceService.GetExchange(exchange)?.GetRates();
            if (prices == null) return BadRequest("Not found");
            return Ok(prices);
        }

        [HttpGet]
        [Route("price/{exchange}/{market}_{currency}")]
        public IActionResult GetPrice(string exchange, string market, string currency)
        {
            var price = _priceService.GetExchange(exchange)?.GetRate(market, currency);
            if (price == null) return BadRequest("Not found");
            return Ok(price);
        }

        [HttpGet]
        [Route("url/{exchange}/{market}_{currency}")] //Add API key and secret???
        public IActionResult GetOrderUrl(string exchange, string market, string currency, OrderType? orderType, decimal? amount)
        {
            if (!orderType.HasValue || !amount.HasValue || amount.Value <= 0) return BadRequest("Incorrect parameters");
            var url = _priceService.GetExchange(exchange)?.GetUrl(orderType.Value, market, currency, amount.Value);
            if (url != null) return Ok(url);
            else return BadRequest("Not found");
        }
    }
}