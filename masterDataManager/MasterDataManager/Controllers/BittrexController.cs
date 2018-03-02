using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Enums;
using DataLayer.Models;
using MasterDataManager.Services;
using MasterDataManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MasterDataManager.Models;

namespace MasterDataManager.Controllers
{

    [Produces("application/json")]
    [Route("api/bittrex")]
    public class BittrexController : Controller

    { }
}
        /*
        private ITradeRepository _tradeRepository;
        private IStrategyRepository _strategyRepository;
        private IBittrexService _bittrexService;
        private IMarketRepository _marketRepository;
        private IUserAssetRepository _assetRepository;

        public BittrexController(
            ITradeRepository tradeRepository, 
            IStrategyRepository strategyRepository,
            IBittrexService bittrexService,
            IMarketRepository marketRepository,
            IUserAssetRepository assetRepository)
        {
            _tradeRepository = tradeRepository;
            _strategyRepository = strategyRepository;
            _bittrexService = bittrexService;
            _marketRepository = marketRepository;
            _assetRepository = assetRepository;
        }

        [HttpGet("trade/{id}")]
        [Authorize]
        public IActionResult CheckState(int id)
        {
            var trade = _tradeRepository.GetById(id);
            if(trade == null)
                return BadRequest("No trade found for given id: db");
            var bittrexTransaction = _bittrexService.GetOrder(trade);
            if (bittrexTransaction == null)
                return BadRequest("No trade found for given id: bittrex");


            if (!trade.Equals(bittrexTransaction))
            {
                trade.TradeState = bittrexTransaction.TradeState;
                trade.QuantityRemaining = bittrexTransaction.QuantityRemaining;
                trade.Closed = bittrexTransaction.Closed;
                trade.Price = bittrexTransaction.Price;

                _tradeRepository.Edit(trade);
            }
            return Ok(new
            {
                success = true,
                trade = trade
            });
        }

        /*
        [HttpPost("sell")]
        public JsonResult Sell(int strategyId, string marketCode, double quantity, double rate)
        {
            var error = CheckBuySellErrors(strategyId, marketCode, quantity, rate, OrderType.Sell);
            if (error  != null) return Json(new JsonResponse
            {
                success = false,
                message = error
            });
            var market = _marketRepository.GetByCode(marketCode);

            var result = _bittrexService.SellOrder(market, quantity, rate);
            if (result == String.Empty) return Json(new JsonResponse
            {
                success = false,
            });
            return Json(new JsonResponse
            {
                success = true,
                response = result,
            });
        }

        [HttpPost("buy")]
        public JsonResult Buy(int strategyId, string marketCode, double quantity, double rate)
        {
            var error = CheckBuySellErrors(strategyId, marketCode, quantity, rate, OrderType.Sell);
            if (error != null) return Json(new JsonResponse
            {
                success = false,
                message = error
            });
            var market = _marketRepository.GetByCode(marketCode);
            var result = _bittrexService.BuyOrder(market, quantity, rate);
            if (result == String.Empty) return Json(new JsonResponse
            {
                success = false,
            });
            return Json(new JsonResponse
            {
                success = true,
                response = result,
            });
        }

        [HttpPost("cancel/{id}")]
        public JsonResult Cancel(int id)
        {
            var trade = _tradeRepository.GetById(id);
            if (trade == null)
                return Json(new JsonResponse{ success = false, message = "No trade found for given id: db" });
            
            var result = _bittrexService.CancelOrder(trade); // Check what happens for nonsense uuid
            if (!result) return Json(new JsonResponse
            {
                success = false,
                message = "Not found on Bittrex"
            });
            return Json(new JsonResponse
            {
                success = true
            });
        }

        private string CheckBuySellErrors(int strategyId, string marketCode, double quantity, double rate, OrderType orderType)
        {
            var strategy = _strategyRepository.GetById(strategyId);
            if (strategy == null) return "Strategy with this id not registered";
            if (strategy.StrategyState == StrategyState.Stopped) return "Strategy stopped";
            if (strategy.StrategyState == StrategyState.Susspended) return "Strategy susspended";
            var market = _marketRepository.GetByCode(marketCode);
            if (market == null) return "Market not found";
            var soldCurrency = orderType == OrderType.Buy ? market.BaseCurrency : market.SecondaryCurrency;
            //var soldAsset = _assetRepository.GetByStrategyId(strategyId).FirstOrDefault(o => o.Currency.Equals(soldCurrency));
            //if (quantity > soldAsset.Amount) return "Not suficient funds";
            return null;
        }
        */
        
    //}
    
///}