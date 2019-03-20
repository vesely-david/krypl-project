using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Infrastructure
{
    public class StrategyAssetRepository : Repository<StrategyAsset>, IStrategyAssetRepository
    {
        public StrategyAssetRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<StrategyAsset> GetByStrategyId(string strategyId)
        {
            return _dbContext.StrategyAssets
                .Include(o => o.UserAsset)
                .Where(o => o.StrategyId == strategyId);
        }
    }
}
