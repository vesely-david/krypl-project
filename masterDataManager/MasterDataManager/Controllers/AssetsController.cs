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
using MasterDataManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace MasterDataManager.Controllers
{
    [Authorize]
    [Route("assets")]
    public class AssetsController : Controller
    {
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private IStrategyRepository _strategyRepository;
        private IUserAssetRepository _userAssetRepository;
        private ITradeRepository _tradeRepository;
        private IExchangeObjectFactory _exchangeFactory;
        private IBalanceService _balanceService;
        private IMapper _mapper;

        public AssetsController(
            UserManager<User> userManager,
            IConfiguration configuration,
            IStrategyRepository strategyRepository,
            IUserAssetRepository userAssetRepository,
            ITradeRepository tradeRepository,
            IExchangeObjectFactory exchangeFactory,
            IBalanceService balanceService,
            IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _strategyRepository = strategyRepository;
            _userAssetRepository = userAssetRepository;
            _tradeRepository = tradeRepository;
            _exchangeFactory = exchangeFactory;
            _balanceService = balanceService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAssets()
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var assets = _userAssetRepository.GetByUserId(userId);
            return Ok(assets.Select(_mapper.Map<JsonUserAssetModel>));
        }

        //[HttpPost("real")]
        //public IActionResult UpdateRealAssets()
        //{
        //    return Ok();
        //}
        [HttpPost("paper")]
        public IActionResult UpdatePaperAssets([FromBody]IEnumerable<JsonAssetModel> assetModels)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var assets = assetModels.Select(_mapper.Map<Asset>);
            var result = _balanceService.UpdateUserAssets(userId, assets, TradingMode.PaperTesting);
            if (result.Success) return Ok();
            return BadRequest("Unexpected db error");
        }
        //[HttpPost("backtest")]
        //public IActionResult UpdateBacktestAssets(IEnumerable<AssetModel> assets, string exchange)
        //{
        //    return Ok();
        //}
    }
}
