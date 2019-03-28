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
using MasterDataManager.Utils;
using Microsoft.AspNetCore.Mvc;


namespace MasterDataManager.Controllers
{
    [Produces("application/json")]
    [Route("strategies")]
    public class OverviewController : Controller
    {

        private IStrategyRepository _strategyRepository;
        private IAssetRepository _assetRepository;
        private IMarketDataService _marketDataService;
        private IMapper _mapper;

        public OverviewController(
            IStrategyRepository strategyRepository,
            IAssetRepository assetRepository,
            IMarketDataService marketDataService,
            IMapper mapper)
        {
            _strategyRepository = strategyRepository;
            _assetRepository = assetRepository;
            _marketDataService = marketDataService;
            _mapper = mapper;
        }


        [HttpGet("{mode}/overview")]
        public IActionResult GetOverview(TradingMode mode)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var strategies = _strategyRepository.GetUserStrategiesByMode(userId, mode);
            var overview = _strategyRepository.GetUserOverviewByMode(userId, mode);

            //var assets = _assetRepository.GetByUserId(userId)
                //.Where(o => o.TradingMode == mode).Select(o => (currency: o.Currency, amount: o.Amount));

            //var currentValue = await _marketDataService.EvaluateAssetSet(assets, "binance");
            //var reserved = strategies.Aggregate(new EvaluationTick(), (res, val) =>
            //{
            //    var eval = val.Evaluations.Last();
            //    res.BtcValue += eval.BtcValue;
            //    res.UsdValue += eval.UsdValue;
            //    return res;
            //});

            return Ok(new
            {
                allOpenededTradesCount = strategies.SelectMany(o => o.Trades).Count(o => !o.Closed.HasValue),
                allTradesCount = strategies.SelectMany(o => o.Trades).Count(),
                allNewTradesCount = strategies.Sum(o => o.GetNewTrades()),
                runningCount = strategies.Count(o => !o.Stop.HasValue),
                allCount = strategies.Count(),
                //currentValue = _mapper.Map<JsonEvaluationModel>(currentValue),
                yesterdayValue = _mapper.Map<JsonEvaluationModel>(overview.GetYesterdayValue()),
                overviewStrategyId = overview.Id,
                //reserved = _mapper.Map<JsonEvaluationModel>(reserved),
            });
        }


        [HttpGet("{mode}")]
        public IActionResult GetStrategies(TradingMode mode, int page=0, int perPage=15, bool runningFirst = true)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var allStrategies = _strategyRepository.GetUserStrategiesByMode(userId, mode).OrderBy(o => o.Start);
            var strategies = allStrategies.Skip(page * perPage).Take(perPage);

            return Ok(new
            {
                strategies = strategies.Select(_mapper.Map<JsonStrategyModel>),
                page,
                perPage,
                count = allStrategies.Count()
            });
        }

        [HttpGet("{mode}/{strategyId}")]
        public IActionResult GetStrategy(TradingMode mode, string strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var strategy = _strategyRepository.GetUserStrategiesByMode(userId, mode).FirstOrDefault(o => o.Id == strategyId);
            if (strategy == null) return BadRequest("Strategy not found");
            return Ok(_mapper.Map<JsonStrategyModel>(strategy));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStrategy([FromBody]StrategyRegistrationModel model) //TODO: Move logic from controller
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");
            if (model.assets == null || !model.assets.Any()) return BadRequest("No asset selected");

            var assets = _assetRepository.GetByUserId(userId).Where(o => 
                o.Exchange.Equals(model.exchange, StringComparison.InvariantCultureIgnoreCase) &&
                o.TradingMode == model.tradingMode &&
                string.IsNullOrEmpty(o.StrategyId));

            var strategyAssets = new List<Asset>();
            var currentPrices = await _marketDataService.GetCurrentPrices(model.exchange);
            var firstEvaluation = new EvaluationTick();

            foreach (var modelAsset in model.assets)
            {
                var asset = assets.FirstOrDefault(o => o.Id.Equals(modelAsset.id));
                if (asset == null || (asset.Amount < modelAsset.amount))
                {
                    return BadRequest("Insufficient funds");
                }
                asset.Amount -= modelAsset.amount;
                strategyAssets.Add(new Asset
                {
                    Amount = modelAsset.amount,
                    Currency = asset.Currency,
                    Exchange = asset.Exchange,
                    TradingMode = model.tradingMode,
                    UserId = userId,
                });
                if(!currentPrices.ContainsKey(asset.Currency)) return BadRequest("Cannot estimate initial value");
                firstEvaluation.BtcValue += currentPrices[asset.Currency].BtcValue * modelAsset.amount;
                firstEvaluation.UsdValue += currentPrices[asset.Currency].UsdValue * modelAsset.amount;
                if (asset.Amount == 0) _assetRepository.DeleteNotSave(asset);
            }

            var strategy = new Strategy
            {
                Name = model.name,
                Description = model.description,
                Start = DateTime.Now,
                StrategyState = StrategyState.Running,
                TradingMode = model.tradingMode,
                IsOverview = false,
                Assets = strategyAssets,
                UserId = userId,
                Evaluations = new List<EvaluationTick> { firstEvaluation }
            };

            _strategyRepository.Add(strategy);
            _assetRepository.Save();
            return Ok(strategy.Id);
        }
    }
}
