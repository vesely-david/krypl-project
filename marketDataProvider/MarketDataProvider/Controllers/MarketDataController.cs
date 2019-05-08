using System;
using MarketDataProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketDataProvider.Controllers
{
    [Route("marketdata")]
    [Produces("application/json")]
    public class MarketDataController : Controller
    {
        private MarketDataService _marketDataService;

        public MarketDataController(MarketDataService markeDataService)
        {
            _marketDataService = markeDataService;
        }


        [HttpGet]
        [Route("marketcal")]
        public IActionResult MarketCalEvents()
        {
            return Ok(_marketDataService.GetProvider("marketCal").GetData());
        }
    }
}
