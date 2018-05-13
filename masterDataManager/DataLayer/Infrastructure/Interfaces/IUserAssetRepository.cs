﻿using DataLayer.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IUserAssetRepository : IRepository<UserAsset>
    {
        //IEnumerable<UserAsset> GetByStrategyId(int strategyId);

        IEnumerable<UserAsset> GetByUserId(string userId);
        //IEnumerable<UserAsset> GetUserAssetsIncludingStrategies();
        IEnumerable<UserAsset> GetByUserAndExchange(string userId, string exchangeId);
    }
}