using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using MasterDataManager.Utils;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Models;

namespace MasterDataManager.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/client")]
    public class ClientController : Controller
    {
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private IStrategyRepository _strategyRepository;
        private IUserAssetRepository _userAssetRepository;
        private IExchangeRepository _exchangeRepository;
        private ICurrencyRepository _currencyRepository;
        private ITradeRepository _tradeRepository;
        private IExchangeObjectFactory _exchangeFactory;
        private IBalanceService _balanceService;

        public ClientController(
            UserManager<User> userManager, 
            IConfiguration configuration,
            IStrategyRepository strategyRepository,
            IUserAssetRepository userAssetRepository,
            IExchangeRepository exchangeRepository,
            ICurrencyRepository currencyRepository,
            ITradeRepository tradeRepository,
            IExchangeObjectFactory exchangeFactory,
            IBalanceService balanceService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _strategyRepository = strategyRepository;
            _userAssetRepository = userAssetRepository;
            _exchangeRepository = exchangeRepository;
            _currencyRepository = currencyRepository;
            _tradeRepository = tradeRepository;
            _exchangeFactory = exchangeFactory;
            _balanceService = balanceService;


        }
        //======== GET STRATEGY LIST =========
        [HttpGet("realStrategies")]
        public IActionResult GetRealStrategiesAsync()
        {
            return GetStrategiesAsync(TradingMode.Real);
        }

        [HttpGet("paperStrategies")]
        public IActionResult GetPaperStrategiesAsync()
        {
            return GetStrategiesAsync(TradingMode.PaperTesting);
        }

        [HttpGet("backTestStrategies")]
        public IActionResult GetBacktestStrategiesAsync()
        {
            return GetStrategiesAsync(TradingMode.BackTesting);
        }
        //======== GET OVERVIEW =========
        [HttpGet("realOverview")]
        public IActionResult GetRealOverview()
        {
            return GetOverview(TradingMode.Real);
        }

        [HttpGet("paperOverview")]
        public IActionResult GetPaperOverview()
        {
            return GetOverview(TradingMode.PaperTesting);
        }

        [HttpGet("backTestOverview")]
        public IActionResult GetBacktestOverview()
        {
            return GetOverview(TradingMode.BackTesting);
        }
        //======== REGISTER STRATEGY =========

        [HttpPost("registerReal")]
        public IActionResult RegisterReal()
        {
            return GetOverview(TradingMode.Real);
        }

        [HttpPost("registerPaper")]
        public IActionResult RegisterPaper()
        {
            return GetOverview(TradingMode.PaperTesting);
        }

        [HttpPost("registerBackTest")]
        public IActionResult RegisterBackTest()
        {
            return GetOverview(TradingMode.BackTesting);
        }

        //======== MANAGE ASSETS =========
        [HttpPost("mirrorRealAssets")]
        public async Task<IActionResult> MirrorRealAssets([FromBody]string exchangeName)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var exchangeService = _exchangeFactory.GetExchange(exchangeName);
            if (exchangeService == null) return BadRequest("Exchange not found");

            var exchangeId = exchangeService.GetExchangeId();
            if (exchangeId == -1) return BadRequest("Unspecific error"); //Should not happen

            var balances = await exchangeService.GetBalances();
            var insufficient = new List<string>();
            var result = _balanceService
                .UpdateUserAssets(balances, userId.Value, exchangeId, TradingMode.Real, out insufficient);

            return result ? (IActionResult)Ok() : (IActionResult)BadRequest(Json(insufficient));
        }
         

        private IActionResult GetStrategiesAsync(TradingMode strategyMode)
        {

            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found" );

            var strategies = _strategyRepository.GetByUserId(userId.Value).Where(o => o.TradingMode == strategyMode);
            var yesterday = DateTime.Now.AddDays(-1);
            /*foreach (var strategy in strategies)
            {
                //TEST!!!
                var sorted = strategy.Evaluation.OrderBy(o => o.TimeStamp);
                var startEvaluation = sorted.First();
                var endEvaluation = sorted.Last();
                var yesterdayEvaluation = sorted.FirstOrDefault(o => o.TimeStamp < yesterday);
            }*/
            var strategyList = strategies.Select(o => new
            {
                id = o.Id,
                name = o.Name,
                description = o.Description,
                openedTradesCount = o.Trades.Select(p => p.Closed == null).ToList().Count,
                tradesCounts = o.Trades.ToList().Count,
                newTradesCount = o.NewTrades,
                changeDayBtc = 12,
                changeDayUsd = -30,
                changeAllBtc = 1,
                changeAllUsd = -19,
                currentValueBtc = 0.0143,
                currentValueUsd = 321,
                lastTrade = _tradeRepository.GetLast(o.Id)
            });

            return Ok(strategyList);
        }

        private IActionResult GetOverview(TradingMode tradingMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            return Ok(GetOverviewObject(userId.Value, tradingMode));
        }

        [HttpGet("mainOverview")]
        public IActionResult GetMainOverview()
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var real = GetOverviewObject(userId.Value, TradingMode.Real);
            var paper = GetOverviewObject(userId.Value, TradingMode.PaperTesting);
            var backtest = GetOverviewObject(userId.Value, TradingMode.BackTesting);

            return Ok(new
            {
                real = real,
                paper = paper,
                backtest = backtest
            });
        }

        [HttpPost("register")]
        public IActionResult RegisterStrategyAsync([FromBody] StrategyRegistrationModel model)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var assets = _userAssetRepository.GetByUserId(userId.Value)
                .Where(o => o.ExchangeId == model.exchangeId)
                .Where(o => o.TradingMode == TradingMode.Real);
            if (assets == null) return BadRequest("No assets found");

            var strategyAssets = new List<StrategyAsset>();
            foreach (var modelAsset in model.assets)
            {                     
                var asset = assets.FirstOrDefault(o => o.CurrencyId == modelAsset.currencyId);
                if(asset == null || (asset.GetFreeAmount() < modelAsset.amount))
                {
                    return BadRequest("Insufficient funds" + asset != null ? " for" + asset.Currency.Name : "");
                }
                else
                {
                    strategyAssets.Add(new StrategyAsset
                    {
                        Amount = modelAsset.amount,
                        UserAsset = asset
                    });
                }
            }

            var strategy = new Strategy
            {
                Name = model.name,
                Description = model.description,
                Start = DateTime.Now,
                StrategyState = StrategyState.Running,
                TradingMode = TradingMode.Real,
                StrategyAssets = strategyAssets,
                NewTrades = 0,
                UserId = userId.Value,
                ExchangeId = model.exchangeId
            };

            _strategyRepository.Add(strategy);
            return Ok(strategy.Id);
        }

        [HttpPost("stopStrategy")]
        public IActionResult StopStrategy(int strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByUserId(userId.Value).FirstOrDefault(o => o.Id == strategyId);
            if (strategy == null) return BadRequest("Strategy not found");

            strategy.StrategyState = StrategyState.Stopped;
            strategy.Stop = DateTime.Now;
            _strategyRepository.Edit(strategy);
            return Ok();
        }

        [HttpGet("getStrategy")]
        public IActionResult GetStrategy(int strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByUserId(userId.Value).FirstOrDefault(o => o.Id == strategyId);
            if (strategy == null) return BadRequest("Strategy not found");

            if(strategy.NewTrades != 0)
            {
                strategy.NewTrades = 0;
                _strategyRepository.Edit(strategy);
            }

            return Ok(new {
                name = strategy.Name,
                mode = strategy.TradingMode,
                description = strategy.Description,
                trades = strategy.Trades,
                evaluation = strategy.Evaluation,
            });
        }

        private IActionResult ForgetAllNewTrades(TradingMode strategyMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var strategies = _strategyRepository.GetByUserId(userId.Value).Where(o => o.TradingMode == strategyMode);

            foreach (var strategy in strategies)
            {
                strategy.NewTrades = 0;
                _strategyRepository.Edit(strategy);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonLoginModel userModel)
        {
            var username = userModel.username;
            var password = userModel.password;
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var securityKey = _configuration["JwtTokens:Secret"];
                var token = JwtTokenUtils.GenerateToken(user, securityKey);
                var validUntil = token.Claims.FirstOrDefault(o => o.Type == "exp")?.Value;
                return Json(new
                {
                    jwt = new JwtSecurityTokenHandler().WriteToken(token),
                    validUntil = validUntil
                });
            }
            return NotFound();
        }

        private object GetOverviewObject(int userId, TradingMode tradingMode)
        {
            var strategies = _strategyRepository.GetByUserId(userId).Where(o => o.TradingMode == tradingMode);
            var userAssets = _userAssetRepository.GetByUserId(userId).Where(o => o.TradingMode == tradingMode);

            var overview = new
            {
                allOpenededTradesCount = strategies.SelectMany(o => o.Trades).Where(o => o.Closed == null).Count(),
                allTradesCount = strategies.SelectMany(o => o.Trades).Count(),
                allNewTradesCount = strategies.Sum(o => o.NewTrades),
                runningStrategiesCount = strategies.Where(o => o.Stop == null).Count(),
                allStrategiesCount = strategies.Count(),
                strategiesAssetBtc = 0.67357385,
                strategiesAssetUsd = 8921.548,
                freeAssetBtc = 0.23754785,
                freeAssetUsd = 3129.3,
                sumAssetBtc = 0.9111217,
                sumAssetUsd = 12050.848,
                changeDayBtc = 9.1,
                changeDayUsd = 12.67,
            };

            var grouped = userAssets.Where(o => o.TradingMode == tradingMode).GroupBy(o => o.Exchange);
            var assets = grouped.Select(group => new
            {
                text = group.Key.Name,
                value = group.Key.Id,
                assets = group.Select(o => new
                {
                    text = o.Currency.Code,
                    value = o.Currency.Id,
                    sum = o.Amount,
                    free = o.GetFreeAmount()
                })
            });
            return new
            {
                overview = overview,
                assets = assets
            };
        }
    }
}
