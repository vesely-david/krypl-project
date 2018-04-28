using DataLayer.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class MarketRepository : Repository<Market>, IMarketRepository
    {
        public MarketRepository(MarketDataContext dbContext) : base(dbContext)
        {
        }

        public Market GetMarketByCurrencies(string firstCurrencyId, string secondCurrencyId)
        {
            return _dbContext.Markets
                .FirstOrDefault(o => (o.BaseCurrencyId == firstCurrencyId  && o.MarketCurrencyId == secondCurrencyId)
                 || (o.BaseCurrencyId == secondCurrencyId && o.MarketCurrencyId == firstCurrencyId));
        }
    }
}
