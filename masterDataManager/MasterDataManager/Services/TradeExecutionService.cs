using System;
using System.Linq;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services
{
    public class TradeExecutionService : ITradeExecutionService
    {
        private IStrategyRepository _strategyRepository;
        private IAssetRepository _assetRepository;
        private ITradeRepository _tradeRepository;
        private IMarketDataService _marketDataService;

        public TradeExecutionService(
            IStrategyRepository strategyRepository,
            IAssetRepository assetRepository,
            ITradeRepository tradeRepository,
            IMarketDataService marketDataService)
        {
            _strategyRepository = strategyRepository;
            _assetRepository = assetRepository;
            _tradeRepository = tradeRepository;
            _marketDataService = marketDataService;
        }

        //public Result PutTradeForCurrentPrice()

        public Result PutOrder(TradeOrder order, string strategyId, OrderType orderType)
        {
            var strategyAssets = _assetRepository.GetByStrategyId(strategyId);
            var coins = order.Symbol.Split('_');
            var soldAsset = strategyAssets.FirstOrDefault(o => 
                !o.IsReserved &&  o.Exchange == order.Exchange && 
                o.Currency == coins[orderType == OrderType.Buy ? 0 : 1]);
            if (soldAsset == null) return new Result(false, "Asset not found");
            if (soldAsset.Amount < order.Amount) return new Result(false, "Insufficient funds for" + soldAsset.Currency);

            var reserverAsset = new Asset(soldAsset)
            {
                Amount = order.Amount,
                IsReserved = true,
                StrategyId = strategyId,
            };
            soldAsset.Amount -= order.Amount;
            _assetRepository.AddNotSave(reserverAsset);
            if (soldAsset.Amount == 0) _assetRepository.DeleteNotSave(soldAsset);
            else _assetRepository.EditNotSave(soldAsset);
            var quantity = orderType == OrderType.Buy ? order.Amount / order.Rate.Value : order.Amount;
            var trade = new Trade
            {
                MarketId = order.Symbol,
                Opened = DateTime.Now,
                Quantity = quantity,
                OrderType = orderType,
                StrategyId = strategyId,
                TradeState = TradeState.New,
                Price = order.Rate.Value,
                QuantityRemaining = quantity,
                ReservedAsset = reserverAsset,
                Total = orderType == OrderType.Buy ? order.Amount : order.Amount * order.Rate.Value,
            };
            _tradeRepository.Add(trade);
            _assetRepository.Save();
            return new Result(true, trade.Id);
        }

        public Result ExecutePaperTrade(Trade trade, decimal rate)
        {
            var strategyAssets = _assetRepository.GetByStrategyId(trade.StrategyId);
            var coins = trade.MarketId.Split('_');
            var boughtCoin = coins[trade.OrderType == OrderType.Buy ? 1 : 0];

            var boughtCoinAsset = strategyAssets.FirstOrDefault(o =>
                !o.IsReserved && o.Exchange == trade.ReservedAsset.Exchange &&
                o.Currency == boughtCoin);
            if(boughtCoinAsset == null)
            {
                _assetRepository.AddNotSave(new Asset
                {
                    Amount = trade.OrderType == OrderType.Buy ? trade.Quantity : trade.Total,
                    Currency = boughtCoin,
                    IsActive = true,
                    IsReserved = false,
                    StrategyId = trade.ReservedAsset.StrategyId,
                    TradingMode = TradingMode.PaperTesting,
                    UserId = trade.ReservedAsset.UserId,
                    Exchange = trade.ReservedAsset.Exchange,
                });
            }
            else
            {
                boughtCoinAsset.Amount += trade.Quantity;
                _assetRepository.EditNotSave(boughtCoinAsset);
            }
            trade.Closed = DateTime.Now;
            trade.TradeState = TradeState.Fulfilled;
            _assetRepository.DeleteNotSave(trade.ReservedAsset);
            trade.ReservedAsset = null;
            _tradeRepository.EditNotSave(trade);

            _tradeRepository.Save();
            _assetRepository.Save();

            return new Result(true, "");
        }

        public Result Cancel(string tradeId) //TODO Execute right away
        {
            var trade = _tradeRepository.GetById(tradeId);
            if (trade == null) return new Result(false, "Trade not found");
            if(trade.TradeState == TradeState.New || trade.TradeState == TradeState.PartialyFulfilled)
            {
                trade.TradeState = trade.TradeState == TradeState.New ? TradeState.NewCanceled : TradeState.PartialyFulfilledCanceled;
                _tradeRepository.Edit(trade);
            }
            return new Result(true, trade.Id);
        }
    }
}
