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
using System.Text;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("client")]
    public class ClientController : Controller
    {
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private IStrategyRepository _strategyRepository;
        private IUserAssetRepository _userAssetRepository;
        private ITradeRepository _tradeRepository;
        private IExchangeObjectFactory _exchangeFactory;
        private IBalanceService _balanceService;

        public ClientController(
            UserManager<User> userManager, 
            IConfiguration configuration,
            IStrategyRepository strategyRepository,
            IUserAssetRepository userAssetRepository,
            ITradeRepository tradeRepository,
            IExchangeObjectFactory exchangeFactory,
            IBalanceService balanceService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _strategyRepository = strategyRepository;
            _userAssetRepository = userAssetRepository;
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
        public IActionResult RegisterReal([FromBody]StrategyRegistrationModel model)
        {
            return RegisterStrategy(model, TradingMode.Real);
        }

        [HttpPost("registerPaper")]
        public IActionResult RegisterPaper([FromBody]StrategyRegistrationModel model)
        {
            return RegisterStrategy(model, TradingMode.PaperTesting);
        }

        [HttpPost("registerBackTest")]
        public IActionResult RegisterBackTest([FromBody]StrategyRegistrationModel model)
        {
            return RegisterStrategy(model, TradingMode.BackTesting);
        }

        //======== MANAGE ASSETS =========
        [HttpPost("mirrorRealAssets/{exchange}")]
        public async Task<IActionResult> MirrorRealAssets(string exchange)
        {
            var userId = HttpContext.User.GetUserId();
            if (String.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var exchangeService = _exchangeFactory.GetExchange(exchange);
            if (exchangeService == null) return BadRequest("Exchange not found");

            var balances = await exchangeService.GetRealBalances(userId);
            _balanceService.UpdateUserAssets(balances, userId, exchange, TradingMode.Real);
            return Ok();
        }

        [HttpPost("mirrorPaperAssets/{exchangeName}")]
        public IActionResult ManagePaperAssets([FromBody]IEnumerable<AssetModel> assets, string exchangeName)
        {
            return MirrorFakeAssets(assets, exchangeName, TradingMode.PaperTesting);
        }

        [HttpPost("mirrorBacktestAssets/{exchangeName}")]
        public IActionResult ManageBacktestAssets(IEnumerable<AssetModel> assets, string exchangeName)
        {
            return MirrorFakeAssets(assets, exchangeName, TradingMode.BackTesting);
        }

        public IActionResult MirrorFakeAssets(IEnumerable<AssetModel> assets, string exchange, TradingMode tradingMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (String.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var balances = assets.Select(o => new Asset
            {
                Amount = o.amount,
                Currency = o.currency
            });

            _balanceService.UpdateUserAssets(balances, userId, exchange, tradingMode);
            return Ok();
        }

        private IActionResult GetStrategiesAsync(TradingMode strategyMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (String.IsNullOrEmpty(userId)) return BadRequest("User not found" );

            var strategies = _strategyRepository.GetByUserId(userId).Where(o => o.TradingMode == strategyMode);

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
            });

            return Ok(strategyList);
        }

        private IActionResult GetOverview(TradingMode tradingMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (String.IsNullOrEmpty(userId)) return BadRequest("User not found");

            return Ok(GetOverviewObject(userId, tradingMode));
        }

        public IActionResult RegisterStrategy(StrategyRegistrationModel model, TradingMode tradingMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == null) return BadRequest("User not found");

            var assets = _userAssetRepository.GetByUserId(userId)
                .Where(o => o.Exchange == model.exchange)
                .Where(o => o.TradingMode == tradingMode);

            var strategyAssets = new List<StrategyAsset>();
            foreach (var modelAsset in model.assets)
            {                     
                var asset = assets.FirstOrDefault(o => o.Currency == modelAsset.currency);
                if(asset == null || (asset.GetFreeAmount() < modelAsset.amount))
                {
                    return BadRequest("Insufficient funds" + asset != null ? " for" + asset.Currency : "");
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
                TradingMode = tradingMode,
                StrategyAssets = strategyAssets,
                NewTrades = 0,
                UserId = userId,
                ExchangeId = model.exchange
            };

            _strategyRepository.Add(strategy);
            return Ok(strategy.Id);
        }

        [HttpPost("stopStrategy")]
        public IActionResult StopStrategy(string strategyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (String.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var strategy = _strategyRepository.GetByUserId(userId).FirstOrDefault(o => o.Id == strategyId);
            if (strategy == null) return BadRequest("Strategy not found");

            strategy.StrategyState = StrategyState.Stopped;
            strategy.Stop = DateTime.Now;
            _strategyRepository.Edit(strategy);
            return Ok();
        }

        [HttpGet("strategyTrades/{strategyId}")]
        public IActionResult GetStrategyOverview(string strategyId)
        {
            var trades = _tradeRepository.GetByStrategyId(strategyId);
            if (trades == null) return BadRequest("Strategy not found");
            return Ok(trades);
        }

        //Cache request for few minutes in browser
        [HttpGet("strategyOverview/{strategyId}")]
        public IActionResult StrategyData(string strategyId)
        {
            var strategy = _strategyRepository.GetOverview(strategyId); //move to service
            if (strategy == null) return BadRequest();
            return Ok(new
            {
                evaluations = strategy.Evaluations,
                overview = strategy // TODO: Finish
            });
        }

        private IActionResult ForgetAllNewTrades(TradingMode strategyMode)
        {
            var userId = HttpContext.User.GetUserId();
            if (String.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var strategies = _strategyRepository.GetByUserId(userId).Where(o => o.TradingMode == strategyMode);

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

        private object GetOverviewObject(string userId, TradingMode tradingMode)
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
                name = group.Key,
                id = group.Key,
                currencies = group.Select(o => new
                {
                    id = o.Currency,
                    name = o.Currency,
                    sum = o.Amount,
                    free = o.GetFreeAmount()
                })
            }).ToList();

            return new
            {
                overview = overview,
                assets = assets
            };
        }
    }
}
