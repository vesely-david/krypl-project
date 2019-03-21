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
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MasterDataManager.Controllers
{

    [Route("trade")]
    public class TradingController : Controller
    {

        private IStrategyRepository _strategyRepository;
        private IAssetRepository _assetRepository;
        private ITradeRepository _tradeRepository;
        private IMarketDataService _marketDataService;
        private IMapper _mapper;

        public TradingController(
            IStrategyRepository strategyRepository,
            IAssetRepository assetRepository,
            ITradeRepository tradeRepository,
            IMarketDataService marketDataService,
            IMapper mapper)
        {
            _strategyRepository = strategyRepository;
            _assetRepository = assetRepository;
            _tradeRepository = tradeRepository;
            _marketDataService = marketDataService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("{strategyId}")]
        public async Task<IActionResult> ExecuteOrder(string strategyId, [FromBody] JsonOrderModel orderModel)
        {
            var strategyAssets = _assetRepository.GetByStrategyId(strategyId);
            var strategy = _strategyRepository.GetById(strategyId);
            var currentRates = await _marketDataService.GetCurrentRates(orderModel.exchange);
            var coins = orderModel.symbol.Split('_');
            if (coins.Length != 2) return BadRequest("Invalid symbol format");
            if(!currentRates.ContainsKey(orderModel.symbol)) return BadRequest("Symbol not found");
            var baseAsset = strategyAssets.FirstOrDefault(o =>
                o.Exchange == orderModel.exchange &&
                o.Currency == coins[0]);

            var secondAsset = strategyAssets.FirstOrDefault(o =>
                o.Exchange == orderModel.exchange &&
                o.Currency == coins[1]);
            var rate = orderModel.rate ?? currentRates[orderModel.symbol];
            if(orderModel.type == OrderType.Buy)
            {
                if (baseAsset == null || baseAsset.Amount < orderModel.amount * rate) return BadRequest("Insuficient funds");
                baseAsset.Amount -= orderModel.amount * rate;
                if(baseAsset.Amount == 0) _assetRepository.DeleteNotSave(baseAsset);
                else _assetRepository.EditNotSave(baseAsset);

                if (secondAsset == null)
                {
                    _assetRepository.AddNotSave(new Asset
                    {
                        Amount = orderModel.amount,
                        Currency = coins[1],
                        Exchange = orderModel.exchange,
                        StrategyId = strategyId,
                        TradingMode = strategy.TradingMode,
                        UserId = strategy.UserId
                    });
                }
                else
                {
                    secondAsset.Amount += orderModel.amount;
                    _assetRepository.EditNotSave(secondAsset);

                }
            }
            else //(orderModel.type == OrderType.Sell
            {
                if (secondAsset == null || secondAsset.Amount < orderModel.amount) return BadRequest("Insuficient funds");
                secondAsset.Amount -= orderModel.amount;
                if (secondAsset.Amount == 0) _assetRepository.DeleteNotSave(secondAsset);
                else _assetRepository.EditNotSave(secondAsset);

                if(baseAsset == null)
                {
                    _assetRepository.AddNotSave(new Asset
                    {
                        Amount = orderModel.amount * rate,
                        Currency = coins[0],
                        Exchange = orderModel.exchange,
                        StrategyId = strategyId,
                        TradingMode = strategy.TradingMode,
                        UserId = strategy.UserId
                    });
                }
                else
                {
                    baseAsset.Amount += orderModel.amount * rate;
                    _assetRepository.EditNotSave(baseAsset);
                }
            }
            var trade = new Trade
            {
                Closed = DateTime.Now,
                MarketId = orderModel.symbol,
                Opened = DateTime.Now,
                Quantity = orderModel.amount,
                OrderType = orderModel.type,
                StrategyId = strategyId,
                TradeState = TradeState.Fulfilled,
                Price = rate,
                Total = rate * orderModel.amount,
                QuantityRemaining = 0,
            };
            _tradeRepository.Add(trade);
            _assetRepository.Save();
            return Ok(trade.Id);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOrder(string id)
        {
            return Ok(_tradeRepository.GetById(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult CancelOrder()
        {

            return Ok();
        }
    }
}
