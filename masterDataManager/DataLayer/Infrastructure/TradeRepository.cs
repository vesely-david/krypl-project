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

        public IEnumerable<Trade> GetByStrategyId(int strategyId)
        {
            return _dbContext.Strategies.Include(o => o.Trades)
                .FirstOrDefault(o => o.Id == strategyId)?.Trades;
        }

        public Trade GetLast(int? strategyId)
        {
            return strategyId.HasValue ?
                _dbContext.Trades.Where(o => o.StrategyId == strategyId)
                    .OrderByDescending(o => o.Opened).FirstOrDefault() :
                _dbContext.Trades.OrderByDescending(o => o.Opened).FirstOrDefault();
        }
    }
}
