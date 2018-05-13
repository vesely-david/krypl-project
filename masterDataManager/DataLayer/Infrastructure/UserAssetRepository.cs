using DataLayer.Enums;
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

        public IEnumerable<UserAsset> GetByUserAndExchange(string userId, string exchangeId)
        {
            return _dbContext.UserAssets
                .Where(o => o.Exchange == exchangeId && o.UserId == userId);
        }

        public IEnumerable<UserAsset> GetByUserId(string userId)
        {
            return _dbContext.UserAssets
                .Include(o => o.StrategyAssets)
                .Where(o => o.UserId == userId);
        }
        /*
public IEnumerable<UserAsset> GetUserAssetsIncludingStrategies()
{
    return _dbContext.UserAssets.Include(o => o.StrategyAssets).Include(o => o.Currency);
}
*/
    }
}
