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

        public Strategy GetByIdForOverview(string strategyId)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .FirstOrDefault(o => o.Id == strategyId);
        }

        public IEnumerable<Strategy> GetAllForEvaluation()
        {
            return _dbContext.Strategies
                .Include(o => o.User)
                .Include(o => o.Trades)
                .Include(o => o.Evaluations)
                .Include(o => o.Assets);
        }

        public IEnumerable<Strategy> GetByUserId(string userId)
        {
            return _dbContext.Strategies
                .Include(o => o.Trades)
                .Include(o => o.Evaluations)
                .Include(o => o.Assets)
                .Where(o => o.UserId == userId);
        }

        public Strategy GetUserOverviewByMode(string userId, TradingMode mode)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .Include(o => o.Assets)
                .FirstOrDefault(o => o.UserId == userId && o.TradingMode == mode && o.IsOverview);
        }

        public IEnumerable<Strategy> GetUserStrategiesByMode(string userId, TradingMode mode)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .Include(o => o.Assets)
                .Include(o => o.Trades)
                .Where(o => o.UserId == userId && o.TradingMode == mode && !o.IsOverview);
        }

        public Strategy GetByIdForEvaluations(string strategyId)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .FirstOrDefault(o => o.Id == strategyId);
        }
        public Strategy GetStrategyWithEvaluationsAndAssets(string strategyId)
        {
            return _dbContext.Strategies
                .Include(o => o.Evaluations)
                .Include(o => o.Assets)
                .FirstOrDefault(o => o.Id == strategyId);
        }
    }
}
