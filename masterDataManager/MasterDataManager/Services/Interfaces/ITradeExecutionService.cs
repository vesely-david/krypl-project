using System;
using System.Threading.Tasks;
using DataLayer.Enums;
using DataLayer.Models;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services.Interfaces
{
    public interface ITradeExecutionService
    {
        Task<Result> PutOrder(TradeOrder order, string strategyId, OrderType orderType);
        Task<Result> Cancel(string tradeId);
        Task<Result> MirrorRealTrades(Strategy strategy);
    }
}
