using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Services.Interfaces;
using MarketDataProvider.Services;

namespace MarketDataProvider.Controllers
{
    [Produces("application/json")]
    public class ClientController : Controller
    {
        private IMarketDataMemCacheService _marketDataService;
        private PriceService _priceService;

        public ClientController(
            IMarketDataMemCacheService memCacheService,
            PriceService priceService)
        {
            _marketDataService = memCacheService;
            _priceService = priceService;
        }

        [HttpGet]
        [Route("exchanges")]
        public IActionResult ExchangeList()
        {
            var exchangeInfo = _marketDataService.ExchangeList().Select(o => new
            {
                id = o.Id,
                name = o.Name,
                history = o.ProvidesFullHistoryData,
                currencies = o.Currencies,
            });

            return Ok(exchangeInfo);
        }

        [HttpGet]
        [Route("exchanges/{exchange}")]
        public IActionResult ExchangeInfo(string exchange)
        {
            var exchangeInfo = _marketDataService.GetExchange(exchange);
            if (exchangeInfo == null) return BadRequest("Exchange not found");
            return Ok(exchangeInfo);
        }
    }
}
