using DataLayer.Enums;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IBalanceService
    {
        bool UpdateUserAssets(List<Asset> assets, int userId, int exchangeId, TradingMode tradingMode, out List<string> insufficient);
    }
}
