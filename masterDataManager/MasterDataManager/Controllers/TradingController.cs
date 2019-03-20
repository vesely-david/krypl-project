using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer.Infrastructure.Interfaces;
using MasterDataManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MasterDataManager.Controllers
{

    [Route("trade")]
    public class TradingController : Controller
    {

        private IStrategyRepository _strategyRepository;
        private IUserAssetRepository _userAssetRepository;
        private IStrategyAssetRepository _strategyAssetRepository;
        private ITradeRepository _tradeRepository;
        private IMarketDataService _marketDataService;
        private IMapper _mapper;

        public TradingController(
            IStrategyRepository strategyRepository,
            IUserAssetRepository userAssetRepository,
            IStrategyAssetRepository strategyAssetRepository,
            ITradeRepository tradeRepository,
            IMarketDataService marketDataService,
            IMapper mapper)
        {
            _strategyRepository = strategyRepository;
            _userAssetRepository = userAssetRepository;
            _strategyAssetRepository = strategyAssetRepository;
            _tradeRepository = tradeRepository;
            _marketDataService = marketDataService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            return Ok();
        }

        [HttpPost]
        [Route("{test}")]
        public IActionResult ExecuteOrder()
        {

            var strategyAssets = _strategyAssetRepository.GetByStrategyId("");
            var currentRates = _marketDataService.GetCurrentRates("binance");
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOrder(string id)
        {
            return Ok(_tradeRepository.GetById(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult CancelOrder()
        {

            return Ok();
        }
    }
}
