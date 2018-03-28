using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Infrastructure
{
    public class UserAssetRepository : Repository<UserAsset>, IUserAssetRepository
    {
        public UserAssetRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<UserAsset> GetByUserId(int userId)
        {
            return _dbContext.UserAssets
                .Include(o => o.StrategyAssets).Include(o => o.Exchange).Include(o => o.Currency)
                .Where(o => o.UserId == userId);
        }

        public IEnumerable<UserAsset> GetByUserAndExchange(int userId, int exchangeId)
        {
            return _dbContext.UserAssets.Include(o => o.Currency)
                .Where(o => o.ExchangeId == exchangeId && o.UserId == userId);
        }

        public IEnumerable<UserAsset> GetUserAssetsIncludingStrategies()
        {
            return _dbContext.UserAssets.Include(o => o.StrategyAssets).Include(o => o.Currency);
        }
    }
}
