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
        private IUserAssetRepository _userAssetRepository;
        private IMarketDataService _marketDataService;
        private IMapper _mapper;

        public OverviewController(
            IStrategyRepository strategyRepository,
            IUserAssetRepository userAssetRepository,
            IMarketDataService marketDataService,
            IMapper mapper)
        {
            _strategyRepository = strategyRepository;
            _userAssetRepository = userAssetRepository;
            _marketDataService = marketDataService;
            _mapper = mapper;
        }


        [HttpGet("{mode}/overview")]
        public IActionResult GetOverview(TradingMode mode)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var strategies = _strategyRepository.GetUserStrategiesByMode(userId, mode);
            var strategyValuesYesterday = strategies.Select(o => o.GetYesterdayValue());

            var overview = _strategyRepository.GetUserOverviewByMode(userId, mode);

            var currentValue = overview.Evaluations.Last();
            var yesterdayValue = overview.GetYesterdayValue();
            var reserved = strategies.Aggregate(new EvaluationTick(), (res, val) =>
            {
                var eval = val.Evaluations.Last();
                res.BtcValue += eval.BtcValue;
                res.UsdValue += eval.UsdValue;
                return res;
            });

            var allOpenededTradesCount = strategies.SelectMany(o => o.Trades).Count(o => !o.Closed.HasValue);
            var allTradesCount = strategies.SelectMany(o => o.Trades).Count();
            var allNewTradesCount = strategies.Sum(o => o.GetNewTrades());
            var runningCount = strategies.Count(o => !o.Stop.HasValue);
            var allCount = strategies.Count();
            var currentVal = _mapper.Map<JsonEvaluationModel>(currentValue);
            var yesterdayVal = _mapper.Map<JsonEvaluationModel>(yesterdayValue);
            var reserv = _mapper.Map<JsonEvaluationModel>(reserved);

            return Ok(new
            {
                allOpenededTradesCount = strategies.SelectMany(o => o.Trades).Count(o => !o.Closed.HasValue),
                allTradesCount = strategies.SelectMany(o => o.Trades).Count(),
                allNewTradesCount = strategies.Sum(o => o.GetNewTrades()),
                runningCount = strategies.Count(o => !o.Stop.HasValue),
                allCount = strategies.Count(),
                currentValue = _mapper.Map<JsonEvaluationModel>(currentValue),
                yesterdayValue = _mapper.Map<JsonEvaluationModel>(yesterdayValue),
                reserved = _mapper.Map<JsonEvaluationModel>(reserved),
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

        [HttpPost]
        public async Task<IActionResult> RegisterStrategy([FromBody]StrategyRegistrationModel model) //TODO: Move logic from controller
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");
            if (model.assets == null || !model.assets.Any()) return BadRequest("No asset selected");

            var assets = _userAssetRepository.GetByUserId(userId)
                .Where(o => o.Exchange.Equals(model.exchange, StringComparison.InvariantCultureIgnoreCase))
                .Where(o => o.TradingMode == model.tradingMode);

            var strategyAssets = new List<StrategyAsset>();
            var currentPrices = await _marketDataService.GetCurrentPrices(model.exchange);
            var firstEvaluation = new EvaluationTick
            {
                BtcValue = 0,
                UsdValue = 0,
                TimeStamp = DateTime.Now,
            };

            foreach (var modelAsset in model.assets)
            {
                var asset = assets.FirstOrDefault(o => o.Id.Equals(modelAsset.id));
                if (asset == null || (asset.GetFreeAmount() < modelAsset.amount))
                {
                    return BadRequest("Insufficient funds");
                }
                if (!currentPrices.ContainsKey(asset.Currency))
                {
                    return BadRequest("Cannot evaluate strategy");
                }
                firstEvaluation.BtcValue += currentPrices[asset.Currency].BtcValue;
                firstEvaluation.UsdValue += currentPrices[asset.Currency].UsdValue;
                strategyAssets.Add(new StrategyAsset
                {
                    Amount = modelAsset.amount,
                    UserAsset = asset
                });
            }

            var strategy = new Strategy
            {
                Name = model.name,
                Description = model.description,
                Start = DateTime.Now,
                StrategyState = StrategyState.Running,
                TradingMode = model.tradingMode,
                IsOverview = false,
                StrategyAssets = strategyAssets,
                UserId = userId,
                ExchangeId = model.exchange,
                Evaluations = new List<EvaluationTick> { firstEvaluation }
            };

            _strategyRepository.Add(strategy);
            return Ok(strategy.Id);
        }
    }
}
