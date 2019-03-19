using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MasterDataManager.Utils;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Models;
using AutoMapper;
using System.Collections.Generic;

namespace MasterDataManager.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("strategy")]
    public class StrategyController : Controller
    {
        private IStrategyRepository _strategyRepository;
        private ITradeRepository _tradeRepository;
        private IMapper _mapper;

        public StrategyController(
            IStrategyRepository strategyRepository,
            IUserAssetRepository userAssetRepository,
            ITradeRepository tradeRepository,
            IExchangeObjectFactory exchangeFactory,
            IBalanceService balanceService,
            IMapper mapper)
        {
            _strategyRepository = strategyRepository;
            _tradeRepository = tradeRepository;
            _mapper = mapper;
        }

        [HttpGet("{strategyId}/history")]
        public IActionResult GetStrategyHistory(string strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByIdForEvaluations(strategyId);
            if (strategy == null) return BadRequest("Strategy not found");

            return Ok(strategy.Evaluations.Select(_mapper.Map<JsonEvaluationModel>));
        }


        [HttpGet("{strategyId}/overview")]
        public IActionResult GetStrategyOverview(string strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByIdForOverview(strategyId);
            var result = _mapper.Map<JsonStrategyModel>(strategy);

            strategy.LastCheck = DateTime.Now;
            _strategyRepository.Edit(strategy);

            return Ok(result);
        }


        [HttpGet("{strategyId}/trades")]
        public IActionResult GetStrategyTrades(string strategyId, int page = 0, int perPage = 15)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var allTrades = _tradeRepository.GetByStrategyId(strategyId);
            var trades = allTrades.OrderBy(o => o.Opened)
                .Skip(page * perPage).Take(perPage);

            return Ok(new
            {
                trades = trades.Select(_mapper.Map<JsonTradeModel>),
                page,
                perPage,
                count = allTrades.Count()
            });
        }


        [HttpPost("{strategyId}/stop")]
        public IActionResult StopStrategy(string strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByUserId(userId).FirstOrDefault(o => o.Id == strategyId);
            if (strategy == null) return BadRequest("Strategy not found");

            if(strategy.StrategyState != StrategyState.Stopped)
            {
                strategy.Stop = DateTime.Now;
                strategy.StrategyState = StrategyState.Stopped;
                _strategyRepository.Edit(strategy);
            }
            return Ok();
        }


        [HttpPost("{strategyId}/changestate")]
        public IActionResult ChangeStrategyMode(string strategyId)
        {
            return BadRequest("Not implemented");
        }
    }
}