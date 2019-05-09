using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Models;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MasterDataManager.Controllers
{

    [Route("trade")]
    public class TradingController : Controller
    {
        private ITradeRepository _tradeRepository;
        private ITradeExecutionService _tradeService;
        private IMarketDataService _marketDataService;
        private IStrategyRepository _strategyRepository;
        private IMapper _mapper;

        public TradingController(
            ITradeRepository tradeRepository,
            ITradeExecutionService tradeExecutionService,
            IMarketDataService marketDataService,
            IStrategyRepository strategyRepository,
            IMapper mapper)
        {
            _tradeRepository = tradeRepository;
            _tradeService = tradeExecutionService;
            _marketDataService = marketDataService;
            _strategyRepository = strategyRepository;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("{strategyId}/buy")]
        public async Task<IActionResult> PutBuyOrder(string strategyId, [FromBody] JsonOrderModel orderModel)
        {
            var currentRates = await _marketDataService.GetCurrentRates(orderModel.exchange);
            if (!currentRates.ContainsKey(orderModel.symbol)) return BadRequest("Cannot get current rate");
            if (!orderModel.rate.HasValue) orderModel.rate = currentRates[orderModel.symbol];
            var result = await _tradeService.PutOrder(_mapper.Map<TradeOrder>(orderModel), strategyId, OrderType.Buy);
            if (result.Success) return Ok(result.Data);
            return BadRequest(result.Data);
        }

        [HttpPost]
        [Route("{strategyId}/sell")]
        public async Task<IActionResult> PutSellOrder(string strategyId, [FromBody] JsonOrderModel orderModel)
        {
            var currentRates = await _marketDataService.GetCurrentRates(orderModel.exchange);
            if (!currentRates.ContainsKey(orderModel.symbol)) return BadRequest("Cannot get current rate");
            if (!orderModel.rate.HasValue) orderModel.rate = currentRates[orderModel.symbol];

            var result = await _tradeService.PutOrder(_mapper.Map<TradeOrder>(orderModel), strategyId, OrderType.Sell);
            if (result.Success) return Ok(result.Data);
            return BadRequest(result.Data);
        }

        [HttpPost]
        [Route("{strategyId}/buy/{timestamp}")]
        public async Task<IActionResult> PutHistoryBuyOrder(string strategyId, [FromBody] JsonOrderModel orderModel, long timestamp)
        {
            if (!orderModel.rate.HasValue)
            {
                var rate = await _marketDataService.GetHistoryRate(orderModel.exchange, orderModel.symbol, timestamp);
                if (rate == null) return BadRequest("Cannot get rate");
                orderModel.rate = rate;
            }
            var result = await _tradeService.PutOrder(_mapper.Map<TradeOrder>(orderModel), strategyId, OrderType.Buy);
            if (result.Success) return Ok(result.Data);
            return BadRequest(result.Data);
        }

        [HttpPost]
        [Route("{strategyId}/sell/{timestamp}")]
        public async Task<IActionResult> PutHistorySellOrder(string strategyId, [FromBody] JsonOrderModel orderModel, long timestamp)
        {
            if (!orderModel.rate.HasValue)
            {
                var rate = await _marketDataService.GetHistoryRate(orderModel.exchange, orderModel.symbol, timestamp);
                if (rate == null) return BadRequest("Cannot get rate");
                orderModel.rate = rate;
            }
            var result = await _tradeService.PutOrder(_mapper.Map<TradeOrder>(orderModel), strategyId, OrderType.Sell);
            if (result.Success) return Ok(result.Data);
            return BadRequest(result.Data);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTrade(string id)
        {
            return Ok(_tradeRepository.GetById(id));
        }

        [HttpGet]
        [Route("{strategyId}/trades")]
        public IActionResult GetTrades(string strategyId)
        {
            var strategy = _strategyRepository.GetById(strategyId);
            if (strategy == null) return BadRequest("Strategy not found");
            if(strategy.TradingMode == TradingMode.Real)
            {
                _tradeService.MirrorRealTrades(strategy);
            }
            return Ok(_tradeRepository.GetByStrategyId(strategyId));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> CancelTrade(string id)
        {
            var result = await _tradeService.Cancel(id);
            if (result.Success) return Ok(result.Data);
            return BadRequest(result.Data);
        }
    }
}
