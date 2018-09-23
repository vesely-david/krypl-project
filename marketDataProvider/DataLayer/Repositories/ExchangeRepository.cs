using DataLayer.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class ExchangeRepository : Repository<Exchange>, IExchangeRepository
    {
        public ExchangeRepository(MarketDataContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Exchange> GetAllWithCurrencies()
        {
            return _dbContext.Exchanges
                .Include(o => o.ExchangeCurrencies).ThenInclude(o => o.Currency);
        }

        public Exchange GetForMemCache(string strategyId)
        {
            return _dbContext.Exchanges
                .Include(o => o.ExchangeCurrencies)
                    .ThenInclude(p => p.Currency)
                .Include(o => o.ExchangeMarkets)
                    .ThenInclude(p => p.Market)
                .FirstOrDefault(o => o.Id == strategyId);
        }
    }
}

