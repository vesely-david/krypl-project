using System;
using System.Linq;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services
{
    public class TradeFinalizationService : ITradeFinalizationService
    {
        private IAssetRepository _assetRepository;
        private ITradeRepository _tradeRepository;

        public TradeFinalizationService(
            IAssetRepository assetRepository,
            ITradeRepository tradeRepository)
        {
            _assetRepository = assetRepository;
            _tradeRepository = tradeRepository;
        }

        public Result CancelTrade(Trade trade)
        {
            var reservedAsset = _assetRepository.GetById(trade.ReservedAssetId);
            var strategyAssets = _assetRepository.GetByStrategyId(trade.StrategyId);
            if (trade.TradeState == TradeState.New || trade.TradeState == TradeState.PartialyFulfilled)
            {
                trade.TradeState = trade.TradeState == TradeState.New ? TradeState.NewCanceled : TradeState.PartialyFulfilledCanceled;
                trade.Closed = DateTime.Now;
                var originAsset = strategyAssets.FirstOrDefault(o =>
                    !o.IsReserved && o.Exchange == reservedAsset.Exchange &&
                    o.Currency == reservedAsset.Currency);
                if (originAsset == null)
                {
                    reservedAsset.IsReserved = false;
                    _assetRepository.EditNotSave(reservedAsset);
                }
                else
                {
                    originAsset.Amount += reservedAsset.Amount;
                    _assetRepository.EditNotSave(originAsset);
                    _assetRepository.DeleteNotSave(reservedAsset);
                }

                _tradeRepository.Edit(trade);
                _assetRepository.Save();
                return new Result(true, trade.Id);
            }
            return new Result(false, "Fulfilled or canceled already");
        }

        public Result ExecuteTrade(Trade trade, decimal rate)
        {
            var strategyAssets = _assetRepository.GetByStrategyId(trade.StrategyId);
            var coins = trade.MarketId.Split('_');
            var boughtCoin = coins[trade.OrderType == OrderType.Buy ? 1 : 0];

            var boughtCoinAsset = strategyAssets.FirstOrDefault(o =>
                !o.IsReserved && o.Exchange == trade.ReservedAsset.Exchange &&
                o.Currency == boughtCoin);
            if (boughtCoinAsset == null)
            {
                _assetRepository.AddNotSave(new Asset
                {
                    Amount = trade.OrderType == OrderType.Buy ? trade.Quantity : trade.Total,
                    Currency = boughtCoin,
                    IsActive = true,
                    IsReserved = false,
                    StrategyId = trade.ReservedAsset.StrategyId,
                    TradingMode = trade.ReservedAsset.TradingMode,
                    UserId = trade.ReservedAsset.UserId,
                    Exchange = trade.ReservedAsset.Exchange,
                });
            }
            else
            {
                boughtCoinAsset.Amount += trade.OrderType == OrderType.Buy ? trade.Quantity : trade.Total;
                _assetRepository.EditNotSave(boughtCoinAsset);
            }
            trade.Closed = DateTime.Now;
            trade.TradeState = TradeState.Fulfilled;
            _assetRepository.DeleteNotSave(trade.ReservedAsset);
            trade.ReservedAsset = null;
            _tradeRepository.EditNotSave(trade);

            _tradeRepository.Save();
            _assetRepository.Save();

            return new Result(true, null);
        }
    }
}
