using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class TradeRepository : Repository<Trade>, ITradeRepository
    {
        public TradeRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }
        public Trade GetByUuid(string uuid)
        {
            return _dbContext.Trades.FirstOrDefault(o => o.ExchangeUuid == uuid);
        }

        public IEnumerable<Trade> GetByStrategyId(string strategyId)
        {
            return _dbContext.Trades.Where(o => o.StrategyId == strategyId);
        }
    }
}
