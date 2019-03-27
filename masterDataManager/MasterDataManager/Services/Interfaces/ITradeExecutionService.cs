using System;
using System.Threading.Tasks;
using DataLayer.Enums;
using DataLayer.Models;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services.Interfaces
{
    public interface ITradeExecutionService
    {
        Result PutOrder(TradeOrder order, string strategyId, OrderType orderType);
        Result Cancel(string tradeId);
        Result ExecutePaperTrade(Trade trade, decimal rate);
    }
}
