using System;
using System.Collections.Generic;
using DataLayer.Models;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IStrategyAssetRepository : IRepository<StrategyAsset>
    {
        IEnumerable<StrategyAsset> GetByStrategyId(string strategyId);
    }
}
