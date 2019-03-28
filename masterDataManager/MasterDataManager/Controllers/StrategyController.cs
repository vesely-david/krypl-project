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
using DataLayer.Models;
using System.Threading.Tasks;

namespace MasterDataManager.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("strategy")]
    public class StrategyController : Controller
    {
        private IStrategyRepository _strategyRepository;
        private IAssetRepository _assetRepository;
        private IEvaluationRepository _evaluationRepository;
        private ITradeRepository _tradeRepository;
        private IMarketDataService _marketDataService;
        private IMapper _mapper;

        public StrategyController(
            IStrategyRepository strategyRepository,
            IAssetRepository assetRepository,
            IEvaluationRepository evaluationRepository,
            ITradeRepository tradeRepository,
            IMarketDataService marketDataService,
            IMapper mapper)
        {
            _strategyRepository = strategyRepository;
            _assetRepository = assetRepository;
            _evaluationRepository = evaluationRepository;
            _tradeRepository = tradeRepository;
            _marketDataService = marketDataService;
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
            var userId = HttpContext.User.GetUserId(); //TODO: Add current holdings
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
        public async Task<IActionResult> StopStrategy(string strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByUserId(userId).FirstOrDefault(o => o.Id == strategyId);
            if (strategy == null) return BadRequest("Strategy not found");
            if (strategy.StrategyState != StrategyState.Running) return BadRequest("Strategy is not running");

            strategy.Stop = DateTime.Now;
            strategy.StrategyState = StrategyState.Stopped;
            var userAssets = _assetRepository.GetByUserId(userId);
            var strategyAssets = userAssets.Where(o => o.StrategyId == strategyId);
            foreach(var asset in strategyAssets)
            {
                var assetOrigin = userAssets.FirstOrDefault(o =>
                    !o.IsReserved && o.IsActive &&
                    o.Currency == asset.Currency && o.TradingMode == asset.TradingMode &&
                    string.IsNullOrEmpty(o.StrategyId) && o.Exchange == asset.Exchange);

                if(assetOrigin != null)
                {
                    assetOrigin.Amount += asset.Amount;
                    _assetRepository.EditNotSave(assetOrigin);

                }
                else
                {
                    _assetRepository.AddNotSave(new Asset(asset));

                }
                asset.IsActive = false;
                _assetRepository.EditNotSave(asset);
            }
            var finalEvaluation = await _marketDataService
                .EvaluateAssetSet(strategyAssets.Select(o => (currency: o.Currency, amount: o.Amount)), "binance");

            finalEvaluation.StrategyId = strategyId;
            finalEvaluation.IsFinal = true;

            _evaluationRepository.AddNotSave(finalEvaluation);
            _strategyRepository.Edit(strategy);
            _assetRepository.Save();
            _strategyRepository.Save(); //TODO piece of work?
            _evaluationRepository.Save();
            return Ok();
        }


        [HttpPost("{strategyId}/changestate")]
        public IActionResult ChangeStrategyMode(string strategyId)
        {
            return BadRequest("Not implemented");
        }
    }
}