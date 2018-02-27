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

        public IEnumerable<Strategy> GetByUserId(int userId)
        {
            return _dbContext.Strategies.Include(o => o.StrategyAssets.Select(p => p.UserAsset))
                .Include(o => o.Trades).Include(o => o.Evaluation).Where(o => o.UserId == userId);
        }

        public IEnumerable<Strategy> GetAllForEvaluation()
        {
            return _dbContext.Strategies.Include(o => o.StrategyAssets.Select(p => p.UserAsset).Select(p => p.Currency))
                .Include(o => o.Trades);
        }
    }
}
