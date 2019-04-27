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
        private IMapper _mapper;

        public TradingController(
            ITradeRepository tradeRepository,
            ITradeExecutionService tradeExecutionService,
            IMarketDataService marketDataService,
            IMapper mapper)
        {
            _tradeRepository = tradeRepository;
            _tradeService = tradeExecutionService;
            _marketDataService = marketDataService;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("{strategyId}/buy")]
        public async Task<IActionResult> PutBuyOrder(string strategyId, [FromBody] JsonOrderModel orderModel)
        {
            var currentRates = await _marketDataService.GetCurrentRates(orderModel.exchange);
            if (!currentRates.ContainsKey(orderModel.symbol)) return BadRequest("Cannot get current rate");
            if (!orderModel.rate.HasValue) orderModel.rate = currentRates[orderModel.symbol];
            var result = _tradeService.PutOrder(_mapper.Map<TradeOrder>(orderModel), strategyId, OrderType.Buy);
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
            var result = _tradeService.PutOrder(_mapper.Map<TradeOrder>(orderModel), strategyId, OrderType.Sell);
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
            return Ok(_tradeRepository.GetByStrategyId(strategyId));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult CancelTrade(string id)
        {
            var result = _tradeService.Cancel(id);
            if (result.Success) return Ok(result.Data);
            return BadRequest(result.Data);
        }
    }
}
