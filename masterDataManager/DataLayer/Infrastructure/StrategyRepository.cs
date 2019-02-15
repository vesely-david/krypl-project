using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class StrategyRepository : Repository<Strategy>, IStrategyRepository
    {
        public StrategyRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Strategy> GetAllForEvaluation()
        {
            return _dbContext.Strategies
                .Include(o => o.Trades)
                .Include(o => o.Evaluations)
                .Include(o => o.StrategyAssets).ThenInclude(p => p.UserAsset);
        }

        public IEnumerable<Strategy> GetByUserId(string userId)
        {
            return _dbContext.Strategies
                .Include(o => o.Trades)
                .Include(o => o.Evaluations)
                .Include(o => o.StrategyAssets).ThenInclude(p => p.UserAsset)
                .Where(o => o.UserId == userId);
        }

        public Strategy GetUserOverviewByMode(string userId, TradingMode mode)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .FirstOrDefault(o => o.UserId == userId && o.TradingMode == mode && o.IsOverview);
        }

        public IEnumerable<Strategy> GetUserStrategiesByMode(string userId, TradingMode mode)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .Include(o => o.StrategyAssets)
                .Where(o => o.UserId == userId && o.TradingMode == mode && !o.IsOverview);
        }
    }
}
