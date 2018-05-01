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

        public Market GetMarketByCurrencies(string marketCurrencyId, string currencyId)
        {
            return _dbContext.Markets
                .FirstOrDefault(o => (o.MarketCurrencyId == marketCurrencyId && o.CurrencyId == currencyId));
        }
    }
}
