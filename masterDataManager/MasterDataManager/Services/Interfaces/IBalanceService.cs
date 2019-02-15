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
        Result UpdateUserAssets(string userId, IEnumerable<Asset> assets, TradingMode tradingMode);
    }
}
