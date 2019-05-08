using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private ITradeFinalizationService _tradeFinalizationService;
        private IExchangeObjectFactory _exchangeFactory;

        public TradeExecutionService(
            IStrategyRepository strategyRepository,
            IAssetRepository assetRepository,
            ITradeRepository tradeRepository,
            IMarketDataService marketDataService,
            ITradeFinalizationService tradeFinalizationService,
            IExchangeObjectFactory exchangeObjectFactory)
        {
            _strategyRepository = strategyRepository;
            _assetRepository = assetRepository;
            _tradeRepository = tradeRepository;
            _marketDataService = marketDataService;
            _tradeFinalizationService = tradeFinalizationService;
            _exchangeFactory = exchangeObjectFactory;
        }

        private Asset GetSoldAsset(TradeOrder order, string strategyId, OrderType orderType)
        {
            var strategyAssets = _assetRepository.GetByStrategyId(strategyId);
            var coins = order.Symbol.Split('_');
            return strategyAssets.FirstOrDefault(o =>
                !o.IsReserved && o.Exchange == order.Exchange &&
                o.Currency == coins[orderType == OrderType.Buy ? 0 : 1]);
        }

        public async Task<Result> PutOrder(TradeOrder order, string strategyId, OrderType orderType)
        {
            var strategy = _strategyRepository.GetById(strategyId);
            if (strategy.TradingMode == TradingMode.Real)
            {
                var soldAsset = GetSoldAsset(order, strategyId, orderType);
                if (soldAsset == null || soldAsset.Amount < order.Amount) return new Result(false, "Insufficient funds for" + soldAsset.Currency);
                var exchange = _exchangeFactory.GetExchange(order.Exchange);
                if (exchange == null) return new Result(false, "Exchange not found");
                var uuid = await exchange.PutOrder(order, orderType, strategy.UserId);
                if(uuid == null) return new Result(false, "Exchange unable to create order");
                return new Result(true, ManageAssets(soldAsset, order, orderType, uuid));
            }
            else if( strategy.TradingMode == TradingMode.BackTesting)
            {
                return new Result(true, ManageAssets(null, order, orderType, ""));
            }
            else
            {
                var soldAsset = GetSoldAsset(order, strategyId, orderType);
                if (soldAsset == null || soldAsset.Amount < order.Amount) return new Result(false, "Insufficient funds for" + soldAsset.Currency);
                return new Result(true, ManageAssets(soldAsset, order, orderType, ""));
            }
        }

        private string ManageAssets(Asset soldAsset, TradeOrder order, OrderType orderType, string uuid)
        {
            var reserverAsset = new Asset(soldAsset)
            {
                Amount = order.Amount,
                IsReserved = true,
                StrategyId = soldAsset.StrategyId,
            };
            soldAsset.Amount -= order.Amount;
            _assetRepository.AddNotSave(reserverAsset);
            if (soldAsset.Amount < 0.00000001m) _assetRepository.DeleteNotSave(soldAsset);
            else _assetRepository.EditNotSave(soldAsset);
            var quantity = orderType == OrderType.Buy ? order.Amount / order.Rate.Value : order.Amount;
            var trade = new Trade
            {
                MarketId = order.Symbol,
                Opened = DateTime.Now,
                Quantity = quantity,
                OrderType = orderType,
                StrategyId = soldAsset.StrategyId,
                TradeState = TradeState.New,
                Price = order.Rate.Value,
                QuantityRemaining = quantity,
                ReservedAsset = reserverAsset,
                Total = orderType == OrderType.Buy ? order.Amount : order.Amount * order.Rate.Value,
                ExchangeUuid = uuid,
                Exchange = order.Exchange,
            };
            _tradeRepository.Add(trade);
            _assetRepository.Save();
            return trade.Id;
        }


        public async Task<Result> Cancel(string tradeId)
        {
            var trade = _tradeRepository.GetById(tradeId);
            if (trade == null) return new Result(false, "Trade not found");
            var strategy = _strategyRepository.GetById(trade.StrategyId);
            if(strategy.TradingMode == TradingMode.Real)
            {
                var exchange = _exchangeFactory.GetExchange(trade.Exchange);
                if (exchange == null) return new Result(false, "Exchange not found");
                var cancelResult = await exchange.CancelOrder(trade.ExchangeUuid, strategy.UserId);
                if (!cancelResult)
                {
                    return new Result(false, "Exchange refused to cancel");
                }
            }
            return _tradeFinalizationService.CancelTrade(trade);
        }


        public async Task<Result> MirrorRealTrades(Strategy strategy)
        {
            var openedTrades = _tradeRepository.GetByStrategyId(strategy.Id)
                .Where(o => o.TradeState == TradeState.New || o.TradeState == TradeState.PartialyFulfilled)
                .GroupBy(o => o.Exchange);
            foreach(var exchange in openedTrades)
            {
                var orders = await _exchangeFactory.GetExchange(exchange.Key).GetOrders(strategy.UserId, exchange);
                foreach(var orderToClose in orders.Where(o => o.close))
                {
                    _tradeFinalizationService.ExecuteTrade(orderToClose.trade, orderToClose.trade.Price);
                }
            }
            return new Result(true, "");
        }
    }
}
