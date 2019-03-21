using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Infrastructure
{
    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        public AssetRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Asset> GetByStrategyId(string strategyId)
        {
            return _dbContext.Assets
                .Where(o => o.StrategyId == strategyId);
        }

        IEnumerable<Asset> IAssetRepository.GetByUserAndExchange(string userId, string exchangeId)
        {
            return _dbContext.Assets
                .Where(o => o.Exchange == exchangeId && o.UserId == userId);
        }

        IEnumerable<Asset> IAssetRepository.GetByUserId(string userId)
        {
            return _dbContext.Assets
                .Where(o => o.UserId == userId);
        }
    }
}
