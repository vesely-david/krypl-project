using DataLayer.Enums;
using DataLayer.Models;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public interface IExchangeService
    {
        Task<List<Asset>> GetRealBalances(string userId);
        Task<string> PutOrder(TradeOrder order, OrderType orderType, string userId);
        Task<bool> CancelOrder(string tradeId, string userId);
        Task<bool> MirrorTrades(string userId);
    }
}
