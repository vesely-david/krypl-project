using DataLayer.Repositories.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories
{
    public class ExchangeMarketRepository : Repository<ExchangeMarket>, IExchangeMarketRepository
    {
        public ExchangeMarketRepository(MarketDataContext dbContext) : base(dbContext)
        {
        }
    }
}
