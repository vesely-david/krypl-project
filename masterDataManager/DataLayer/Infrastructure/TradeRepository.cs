using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Enums;

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

        public IEnumerable<Trade> GetOpenedPaperTrades()
        {
            return _dbContext.Trades.Include(o => o.Strategy).Include(o => o.ReservedAsset)
                .Where(o => o.Strategy.TradingMode == TradingMode.PaperTesting && o.TradeState == TradeState.New);
        }

        public IEnumerable<Trade> GetOpenedRealTrades()
        {
            return _dbContext.Trades.Include(o => o.Strategy).Include(o => o.ReservedAsset)
                .Where(o => o.Strategy.TradingMode == TradingMode.Real && (o.TradeState == TradeState.New || o.TradeState == TradeState.PartialyFulfilled)) ;
        }

        public IEnumerable<Trade> GetByUserId(string userId)
        {
            return _dbContext.Trades.Include(o => o.Strategy).Include(o => o.ReservedAsset)
                .Where(o => o.Strategy.UserId == userId);
        }
    }
}
