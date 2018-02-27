using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class MarketRepository : Repository<Market>, IMarketRepository
    {
        public MarketRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }

        public Market GetMarketByCurrencies(int firstCurrencyId, int secondCurrencyId)
        {
            return _dbContext.Markets
                .FirstOrDefault(o => (o.BaseCurrencyId == firstCurrencyId  && o.SecondaryCurrencyId == secondCurrencyId)
                 || (o.BaseCurrencyId == secondCurrencyId && o.SecondaryCurrencyId == firstCurrencyId));
        }

        public Market GetByCode(string marketCode)
        {
            return _dbContext.Markets //.Include(o => o.BaseCurrency).Include(o => o.SecondaryCurrency)
                .FirstOrDefault(o => o.Code.Equals(marketCode, StringComparison.OrdinalIgnoreCase));
        }
    }
}
