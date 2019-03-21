using DataLayer.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IAssetRepository : IRepository<Asset>
    {
        IEnumerable<Asset> GetByStrategyId(string strategyId);
        IEnumerable<Asset> GetByUserId(string userId);
        IEnumerable<Asset> GetByUserAndExchange(string userId, string exchangeId);
    }
}
