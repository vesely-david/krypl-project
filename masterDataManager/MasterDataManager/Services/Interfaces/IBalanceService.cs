using DataLayer.Enums;
using MasterDataManager.Models;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IBalanceService
    {
        void UpdateUserAssets(IEnumerable<Asset> assets, string userId, string exchange, TradingMode tradingMode);
    }
}
