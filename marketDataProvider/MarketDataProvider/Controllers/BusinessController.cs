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
        private HistoryPriceService _historyPriceService;

        public BusinessController(
            IMarketDataMemCacheService memCacheService,
            PriceService priceService,
            HistoryPriceService historyPriceService)
        {
            _marketDataService = memCacheService;
            _priceService = priceService;
            _historyPriceService = historyPriceService;
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
            var binancePrices = _priceService.GetExchange("binance")?.GetValues();
            var poloniexPrices = _priceService.GetExchange("poloniex")?.GetValues();
            return Ok(new { binance = binancePrices, poloniex = poloniexPrices });
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
        [Route("orderbook/{exchange}/{market}")]
        public async Task<IActionResult> GetOrderBook(string exchange, string market)
        {
            var exchangeObj = _priceService.GetExchange(exchange);
            if(exchangeObj == null) return BadRequest("Exchange Not found");
            var orderBook = await exchangeObj.GetOrderBook(market);
            if (orderBook == null) return BadRequest("Not found");
            return Ok(orderBook);
        }


        [HttpGet]
        [Route("rate/{exchange}/{market}_{currency}/{timestamp}")]
        public async Task<IActionResult> GetHistoryRate(string exchange, string market, string currency, long timestamp)
        {
            var exchangeObj = _historyPriceService.GetExchange(exchange);
            if(exchangeObj != null)
            {
                var result = await exchangeObj.GetHistoryRate(market, currency, timestamp);
                if (result == null) return BadRequest();
                return Ok(result);
            }
            return BadRequest("Not found");
        }

        [HttpGet]
        [Route("price/{exchange}/{market}_{currency}")]
        public IActionResult GetPrice(string exchange, string market, string currency)
        {
            var price = _priceService.GetExchange(exchange)?.GetRate(market, currency);
            if (price == null) return BadRequest("Not found");
            return Ok(price);
        }
    }
}