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
        private IAssetRepository _assetRepository;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private IExchangeObjectFactory _exchangeFactory;
        private IMapper _mapper;

        public AssetsController(
            IAssetRepository assetRepository,
            IExchangeSecretRepository exchangeSecretRepository,
            IExchangeObjectFactory exchangeFactory,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _exchangeSecretRepository = exchangeSecretRepository;
            _exchangeFactory = exchangeFactory;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAssets()
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var assets = _assetRepository.GetAllByUserId(userId);
            return Ok(assets.Select(_mapper.Map<JsonAssetModel>));
        }

        [HttpPost("real/{exchangeId}")]
        public async Task<IActionResult> UpdateRealAssets(string exchangeId)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var userAssets = await _exchangeFactory.GetExchange(exchangeId).GetRealBalances(userId);
            var originalAssets = _assetRepository.GetByUserAndExchange(userId, exchangeId).Where(o => o.TradingMode == TradingMode.Real);

            foreach(var asset in userAssets)
            {
                var reservedFromAsset = originalAssets.Where(o => o.Currency == asset.Currency && o.StrategyId != null).Sum(o => o.Amount);
                var originalFreeAsset = originalAssets.FirstOrDefault(o => o.Currency == asset.Currency && o.StrategyId == null);

                if(originalFreeAsset == null)
                {
                    if(asset.Amount != reservedFromAsset) // In order to NOT create empty asset
                    {
                        asset.Amount = asset.Amount - reservedFromAsset;
                        _assetRepository.AddNotSave(asset);
                    }
                }
                else
                {
                    originalFreeAsset.Amount = asset.Amount - reservedFromAsset;
                    _assetRepository.EditNotSave(originalFreeAsset);
                }
            }
            _assetRepository.Save();
            return Ok();
        }

        [HttpPost("paper")]
        public IActionResult UpdatePaperAssets([FromBody]IEnumerable<JsonAssetModel> assetModels)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var assets = assetModels.Select(_mapper.Map<Asset>);
            var userAssets = _assetRepository.GetByUserId(userId).Where(o =>
                o.TradingMode == TradingMode.PaperTesting &&
                string.IsNullOrEmpty(o.StrategyId));

            var toDelete = userAssets.Where(o => !assets.Any(p => p.Id == o.Id));
            foreach(var item in toDelete)
            {
                _assetRepository.DeleteNotSave(item);
            }
            foreach(var item in assets)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    _assetRepository.Add(new Asset
                    {
                        Amount = item.Amount,
                        Currency = item.Currency,
                        Exchange = item.Exchange,
                        TradingMode = TradingMode.PaperTesting,
                        UserId = userId,
                        IsActive = true,
                    });
                }
                else
                {
                    var originalAsset = userAssets.FirstOrDefault(o => o.Id == item.Id);
                    if(originalAsset == null) return BadRequest("Asset not found");
                    originalAsset.Amount = item.Amount;
                    _assetRepository.EditNotSave(originalAsset);
                }
            }
            _assetRepository.Save();
            return Ok();
        }

        //[HttpPost("backtest")]
        //public IActionResult UpdateBacktestAssets(IEnumerable<AssetModel> assets, string exchange)
        //{
        //    return Ok();
        //}
    }
}
